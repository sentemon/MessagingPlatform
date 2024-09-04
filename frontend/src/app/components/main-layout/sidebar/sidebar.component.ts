import {Component, EventEmitter, Output} from '@angular/core';
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
  @Output() chatSelected = new EventEmitter<string>();

  constructor(private chatService: ChatService) { }

  public ngOnInit(): void {
    this.loadChats();
  }

  public onSelectChat(chatId: string): void {
    this.chatSelected.emit(chatId);
  }

  private loadChats(): void {
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
