import { Component } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ChatService } from '../../services/chat/chat.service';
import { ChatSidebar } from '../../models/chatsidebar';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  chats: ChatSidebar[] = [];

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
