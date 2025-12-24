import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  public deleteMessage(chatId: string, messageId: string): Observable<any> {
    const baseUrl = environment.apiUrl;
    return this.http.delete(`${baseUrl}/chats/${chatId}/messages/${messageId}`, { withCredentials: true });
  }
}
