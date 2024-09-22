import {Component, EventEmitter, Output} from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ChatService } from '../../../services/chat/chat.service';
import { ChatSidebar } from '../../../models/chatsidebar';
import {FormsModule} from "@angular/forms";

// ToDo: use SOLID
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, NgOptimizedImage, FormsModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  chats: ChatSidebar[] = [];
  @Output() chatSelected = new EventEmitter<string>();
  isCreatingChat = false;
  newChat = { title: '', usernames: '', chatType: 0 };

  constructor(private chatService: ChatService) { }

  public ngOnInit(): void {
    this.loadChats();
  }

  public onSelectChat(chatId: string): void {
    this.chatSelected.emit(chatId);
  }

  onNewChat() {
    this.isCreatingChat = true;
  }

  closeModal() {
    this.isCreatingChat = false;
  }

  createNewChat() {
    const usernamesArray = this.newChat.usernames.split(',').map(user => user.trim());
    const createChatDto = {
      title: this.newChat.title,
      users: usernamesArray,
      chatType: this.newChat.chatType
    };

    this.chatService.createChat(createChatDto).subscribe(
      response => {
        console.log('Chat created successfully', response);
        this.loadChats();

        const newChatId = response.replace(/(^"|"$)/g, ''); // remove quotes
        console.log('New chat id', newChatId);
        this.onSelectChat(newChatId);

        this.closeModal();
      },
      error => {
        console.error('Error creating chat', error);
      }
    );
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
