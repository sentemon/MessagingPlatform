import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatDto } from "../../models/chatdto";
import { ChatSidebar } from "../../models/chatsidebar";


@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:8080/api/Chat/';

  constructor(private http: HttpClient) { }

  getChats(): Observable<ChatSidebar[]> {
    return this.http.get<ChatSidebar[]>(`${this.apiUrl}getall/`, { withCredentials: true });
  }

  getChat(id: string): Observable<ChatDto> {
    return this.http.get<ChatDto>(`${this.apiUrl}getchat/`, { params: { id }, withCredentials: true });
  }
}
