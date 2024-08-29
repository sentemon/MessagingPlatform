import { Component } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import { ChatService } from '../services/chat/chat.service';
import { Chat } from '../models/chat';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  chats: Chat[] = [];

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.loadChats();
  }

  loadChats(): void {
    this.chatService.getChats().subscribe({
      next: (data: any) => {
        if (data && data.$values && Array.isArray(data.$values)) {
          this.chats = data.$values;
        } else {
          console.error("Wrong data", data);
        }
      },
      error: (error) => {
        console.error('Error loading chats', error);
      }
    });
  }
}
