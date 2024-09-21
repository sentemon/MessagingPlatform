import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ChatDto } from "../../models/chatdto";
import { ChatSidebar } from "../../models/chatsidebar";
import * as signalR from "@microsoft/signalr";
import { AddMessageDto } from "../../models/addmessagedto";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:8080/api/Chat';
  private hubConnection: signalR.HubConnection | null = null;

  constructor(private http: HttpClient) {
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

  public addMessage(addMessageDto: AddMessageDto): Observable<any> {
    return this.http.post("http://localhost:8080/api/Message/add", addMessageDto, { withCredentials: true });
  }

  public deleteMessage(senderId: string, messageId: string): Observable<any> {
    return this.http.delete("http://localhost:8080/api/Message/delete", { params: { senderId, messageId }, withCredentials: true });
  }

  getChats(): Observable<ChatSidebar[]> {
    return this.http.get<ChatSidebar[]>(`${this.apiUrl}/getall/`, { withCredentials: true });
  }

  getChat(id: string | null): Observable<ChatDto> {
    if (id !== null) {
      return this.http.get<ChatDto>(`${this.apiUrl}/getchat/`, { params: { id }, withCredentials: true });
    } else {
      return of(new ChatDto());
    }
  }

  public createChat(createChatDto: { users: string[], title: string, chatType: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, createChatDto, { withCredentials: true, responseType: 'text' });
  }


}
