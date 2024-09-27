import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ChatDto } from "../../models/chatdto";
import { ChatSidebar } from "../../models/chatsidebar";
import * as signalR from "@microsoft/signalr";
import { AddMessageDto } from "../../models/addmessagedto";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: signalR.HubConnection | null = null; // ToDo: maybe move to separate service

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

  public getAll(): Observable<ChatSidebar[]> {
    return this.http.get<ChatSidebar[]>(`${environment.apiUrl}/Chat/getall/`, { withCredentials: true });
  }

  public get(id: string | null): Observable<ChatDto> {
    if (id !== null) {
      return this.http.get<ChatDto>(`${environment.apiUrl}/Chat/get/`, { params: { id }, withCredentials: true });
    } else {
      return of(new ChatDto());
    }
  }

  public create(createChatDto: { users: string[], title: string, chatType: number }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/Chat/create`, createChatDto, { withCredentials: true, responseType: 'text' });
  }

  public delete(id: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Chat/delete`, { params: { id }, withCredentials: true, responseType: 'text' });
  }



}
