import { Injectable } from '@angular/core';
import { Chat } from "../../models/chat";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'http://localhost:8080/api/Chat';

  constructor(private http: HttpClient, private router: Router) { }

  getChats() : Observable<Chat[]> {
    return  this.http.get<Chat[]>(`${this.apiUrl}/getall`, { withCredentials: true });
  }
}
