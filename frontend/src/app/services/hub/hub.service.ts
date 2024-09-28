import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class HubService {
  private hubConnection: signalR.HubConnection | null = null;

  constructor() {
    this.startConnection();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:8080/chat", { withCredentials: true })
      .build();

    this.hubConnection
      .start()
      .catch(err => console.error("Error while starting connection: " + err));
  }

  public sendMessageToUser(username: string, message: string) {
    if (this.hubConnection) {
      this.hubConnection.invoke("SendMessageToUser", username, message)
        .catch(err => console.error("Error while sending message to user: " + err));
    }
  }

  public onReceiveMessage(callback: (user: string, message: string) => void) {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveMessage', callback);
    }
  }
}
