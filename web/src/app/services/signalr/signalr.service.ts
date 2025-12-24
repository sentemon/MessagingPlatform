import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection | null = null;
  public messageReceived: Subject<{ sender: string, content: string }> = new Subject();

  constructor() {
    this.startConnection();
    this.registerReceiveMessage();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, { withCredentials: true })
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.error('Error while starting connection: ' + err));
  }

  public sendMessage(chatId: string, message: string): Observable<void> {
    if (this.hubConnection) {
      const addMessageDto = {
        chatId: chatId,
        content: message
      };

      return new Observable<void>((observer) => {
        this.hubConnection?.invoke('SendMessageToChat', addMessageDto)
          .then(() => {
            observer.next();
            observer.complete();
          })
          .catch(err => {
            console.error('Error while sending message: ' + err);
            observer.error(err);
          });
      });
    } else {
      return new Observable<void>((observer) => {
        observer.error('Hub connection is not established.');
      });
    }
  }

  private registerReceiveMessage() {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveMessage', (sender: string, message: any) => {
        this.messageReceived.next({ sender, content: message.content });
      });
    }
  }
}
