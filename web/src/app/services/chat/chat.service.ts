import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ChatDto } from "../../models/responses/chatdto";
import { ChatSidebar } from "../../models/responses/chatsidebar";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public getAll(): Observable<ChatSidebar[]> {
    return this.http.get<ChatSidebar[]>(`${this.baseUrl}/chats`, { withCredentials: true });
  }

  public get(id: string | null): Observable<ChatDto> {
    if (id !== null) {
      return this.http.get<ChatDto>(`${this.baseUrl}/chats/${id}`, { withCredentials: true });
    } else {
      return of(new ChatDto());
    }
  }

  public create(createChatDto: { title: string; usernames: string[]; chatType: number }): Observable<any> {
    return this.http.post<string>(`${this.baseUrl}/chats`, createChatDto, { withCredentials: true });
  }

  // ToDo: unexpected error like the chats are not loaded after the chat is deleted
  // public delete(id: string): Observable<any> {
  //   return this.http.delete(`${environment.apiUrl}/Chat/delete`, { params: { id }, withCredentials: true, responseType: 'text' });
  // }
}
