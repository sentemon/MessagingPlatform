import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  public deleteMessage(senderId: string, messageId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Message/delete`, { params: { senderId, messageId }, withCredentials: true });
  }
}
