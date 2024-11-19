import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { KeycloakService } from 'keycloak-angular';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  constructor(private readonly keycloakService: KeycloakService) {
    console.log('Setup here!');
    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .withUrl('http://localhost:5179/hubs/userHub', {
        accessTokenFactory: async () => {
          return await this.keycloakService.getToken();
        },
      })
      .build();
  }

  startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  receiveMessage(methodName: string): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on(methodName, (message: string) => {
        observer.next(message);
      });
    });
  }

  sendMessage(message: string): void {
    this.hubConnection.invoke('SendMessage', message);
  }
}
