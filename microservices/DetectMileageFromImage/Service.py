import pika

print("Hello World")

credentials = pika.PlainCredentials('user', 'password')
parameters = pika.ConnectionParameters('localhost', 5672,'/', credentials)

connection = pika.BlockingConnection(parameters)
channel = connection.channel()

for method_frame, properties, body in channel.consume('Test'):
    # Display the message parts and acknowledge the message
    # print(method_frame, properties, body)
    print(body)
    channel.basic_ack(method_frame.delivery_tag)

    # Escape out of the loop after 10 messages
    if method_frame.delivery_tag == 100:
        break

# Cancel the consumer and return any pending messages
requeued_messages = channel.cancel()
print('Requeued %i messages' % requeued_messages)
connection.close()