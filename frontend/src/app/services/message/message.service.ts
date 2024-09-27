import { Injectable } from '@angular/core';
import {AddMessageDto} from "../../models/addmessagedto";
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  public addMessage(addMessageDto: AddMessageDto): Observable<any> {
    return this.http.post(`${environment.apiUrl}/Message/add`, addMessageDto, { withCredentials: true });
  }

  public deleteMessage(senderId: string, messageId: string): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/Message/delete`, { params: { senderId, messageId }, withCredentials: true });
  }
}
