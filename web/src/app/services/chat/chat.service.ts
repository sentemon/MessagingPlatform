import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
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
      return this.http.get<ChatDto>(`${this.baseUrl}/chats/${id}`, { withCredentials: true }).pipe(
        map((chat: any) => ({
          id: chat.id ?? chat.Id,
          chatType: chat.chatType ?? chat.ChatType,
          userChats: chat.userChats ?? chat.UserChats,
          messages: chat.messages ?? chat.Messages,
          creatorId: chat.creatorId ?? chat.CreatorId,
          creator: chat.creator ?? chat.Creator,
          title: chat.title ?? chat.Title
        } as ChatDto))
      );
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
