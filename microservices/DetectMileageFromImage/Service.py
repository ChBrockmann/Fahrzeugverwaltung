import pika
import json
import os
import requests
import random
from typing import Union
from dotenv import load_dotenv
from pika.adapters.blocking_connection import BlockingChannel

load_dotenv()

def send_to_openai(image_as_base64):
    api_key = os.environ.get('API_KEY')

    if api_key is None:
        raise ValueError("API_KEY not found in environment variables")

    query_text = "Was ist der Gesamtkilometerstand in km? Gib nur das Ergebnis ohne Nachkommastellen oder Einheiten an."

    headers = {
        "Content-Type": "application/json",
        "Authorization": f"Bearer {api_key}"
    }
    payload = {
        "model": "gpt-4o-mini",
        "messages": [
            {
                "role": "user",
                "content": [
                    {
                        "type": "text",
                        "text": query_text
                    },
                    {
                        "type": "image_url",
                        "image_url": {
                            "url": f"data:image/jpeg;base64,{image_as_base64}"
                        }
                    }
                ]
            }
        ]
    }

    response = requests.post("https://api.openai.com/v1/chat/completions", headers=headers, json=payload)
    print(response.json())
    detected_total_mileage = response.json()['choices'][0]['message']['content']

    return detected_total_mileage

def callback(ch: Union[BlockingChannel, BlockingChannel], method, properties, body):
    message = json.loads(body)['message']

    image_as_base64 = message.get('imageAsBase64', '')
    image_type = message.get('imageType', '')

    print("Received Image")
    detected_total_mileage = send_to_openai(image_as_base64)
    # detected_total_mileage = f"{random.randint(1, 1000)}"

    if isinstance(detected_total_mileage, str):
        detected_total_mileage = int(detected_total_mileage)

    print (detected_total_mileage)



    # Send the detected total mileage to the next service
    exchange = 'Contracts:LogbookImageAnalyzed'
    message = {
        "messageType": ["urn:message:Contracts:LogbookImageAnalyzed"],
        "message": {
            "detectedTotalMileage": detected_total_mileage
        }
    }
    ch.basic_publish(exchange=exchange,
                     routing_key='',
                     body=json.dumps(message))


# Verbindung zu RabbitMQ herstellen
credentials = pika.PlainCredentials('user', 'password')
parameters = pika.ConnectionParameters('localhost', 5672,'/', credentials)
connection = pika.BlockingConnection(parameters)
channel = connection.channel()

#Create a queue, connect it to the exchange and listen to events
queue = channel.queue_declare(queue='')
channel.queue_bind(queue=queue.method.queue, exchange='Contracts:LogbookEntryCreatedEvent')
channel.basic_consume(queue=queue.method.queue, on_message_callback=callback, auto_ack=True)

print('Waiting for messages...')
channel.start_consuming()