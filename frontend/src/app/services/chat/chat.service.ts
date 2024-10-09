import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ChatDto } from "../../models/responses/chatdto";
import { ChatSidebar } from "../../models/responses/chatsidebar";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient) { }

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

  public create(createChatDto: { title: string; usernames: string[]; chatType: number }): Observable<any> {
    return this.http.post(`${environment.apiUrl}/Chat/create`, createChatDto, { withCredentials: true, responseType: 'text' });
  }

  // ToDo: unexpected error like the chats are not loaded after the chat is deleted
  // public delete(id: string): Observable<any> {
  //   return this.http.delete(`${environment.apiUrl}/Chat/delete`, { params: { id }, withCredentials: true, responseType: 'text' });
  // }
}
