import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatDto } from "../../models/chatdto";
import { ChatSidebar } from "../../models/chatsidebar";
import * as signalR from "@microsoft/signalr"
import { AddMessageDto } from "../../models/addmessagedto";


@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:8080/api/Chat/';
  private hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.apiUrl)
      .build();
  }

  public startConnection() {
    this.hubConnection
      .start()
      .catch(err => console.error("Error while starting connection: " + err));
  }

  public sendMessageToUser(username: string, message: string) {
    this.hubConnection.invoke("SendMessageToUser", username, message)
      .catch(err => console.error("Error while sending message to user: " + err));
  }

  public onReceiveMessage(callback: (user: string, message: string) => void) {
    this.hubConnection.on('ReceiveMessage', callback);
  }

  public addMessage(addMessageDto: AddMessageDto): Observable<any> {
    return this.http.post("http://localhost:8080/api/Message/add", addMessageDto, { withCredentials: true }); // ToDo: fix Single Responsibility Principle
  }

  getChats(): Observable<ChatSidebar[]> {
    return this.http.get<ChatSidebar[]>(`${this.apiUrl}getall/`, { withCredentials: true });
  }

  getChat(id: string | null): Observable<ChatDto> {
    if (id !== null) {
      return this.http.get<ChatDto>(`${this.apiUrl}getchat/`, {params: {id}, withCredentials: true});
    } else {
      return new Observable<ChatDto>();
    }
  }
}
