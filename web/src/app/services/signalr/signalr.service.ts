import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnectionState, LogLevel } from '@microsoft/signalr';
import { Observable, Subject, from } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection | null = null;
  private handlersRegistered = false;
  public messageReceived: Subject<{ chatId: string; sender: string; content: string; sentAt?: string; updatedAt?: string; senderMeta?: { id?: string; firstName?: string; lastName?: string; username?: string } }> = new Subject();

  constructor() {}

  public connect(): Promise<void> {
    if (this.hubConnection && this.hubConnection.state === HubConnectionState.Connected) {
      return Promise.resolve();
    }

    if (!this.hubConnection) {
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(environment.hubUrl, { withCredentials: true })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();
    }

    if (!this.handlersRegistered) {
      this.registerReceiveMessage();
      this.handlersRegistered = true;
    }

    if (this.hubConnection.state === HubConnectionState.Connecting || this.hubConnection.state === HubConnectionState.Reconnecting) {
      return Promise.resolve();
    }

    return this.hubConnection.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => {
        console.error('Error while starting connection: ' + err);
        throw err;
      });
  }

  public sendMessage(chatId: string, message: string): Observable<void> {
    const addMessageDto = { chatId, content: message };

    return from(this.connect()).pipe(
      switchMap(() => new Observable<void>((observer) => {
        this.hubConnection?.invoke('SendMessageToChat', addMessageDto)
          .then(() => {
            observer.next();
            observer.complete();
          })
          .catch(err => {
            console.error('Error while sending message: ' + err);
            observer.error(err);
          });
      }))
    );
  }

  private registerReceiveMessage() {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveMessage', (sender: string, message: any) => {
        const chatId = message?.chatId ?? message?.ChatId ?? '';
        const content = message?.content ?? message?.Content ?? '';
        const sentAt = message?.sentAt ?? message?.SentAt;
        const updatedAt = message?.updatedAt ?? message?.UpdatedAt;
        const senderMeta = message?.sender ?? message?.Sender;
        this.messageReceived.next({ chatId, sender, content, sentAt, updatedAt, senderMeta });
      });

      this.hubConnection.on('ReceiveError', (error: string) => {
        console.error('SignalR hub error:', error);
      });
    }
  }
}
