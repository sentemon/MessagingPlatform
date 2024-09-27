import {Component, EventEmitter, Output} from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ChatService } from '../../../services/chat/chat.service';
import { ChatSidebar } from '../../../models/chatsidebar';
import {FormsModule} from "@angular/forms";
import {AuthService} from "../../../services/auth/auth.service";
import {UserDto} from "../../../models/userdto";
import {Observable} from "rxjs";

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
  activeChatId: string | null = null;
  newChat = { title: '', usernames: '', chatType: 0 };
  user: UserDto | undefined;

  constructor(private chatService: ChatService, private authService: AuthService) {
    this.authService.get().subscribe({
      next: (data: UserDto) => {
        if (data) {
          this.user = data;
        } else {
          console.error("Wrong data", data);
        }
      },
      error: (error) => {
        console.error('Error loading user', error);
      }
    });
  }


  public ngOnInit(): void {
    this.loadChats();
  }

  onSettings() {
    console.log('Settings');
  }
  public onSelectChat(chatId: string): void {
    this.chatSelected.emit(chatId);
  }

  onNewChat() {
    this.isCreatingChat = true;
  }

  toggleMenu(chatId: string, event: MouseEvent) {
    event.stopPropagation();
    this.activeChatId = this.activeChatId === chatId ? null : chatId;
  }

  onEdit(chatId: string) {
    console.log('Editing chat:', chatId);
    this.activeChatId = null;
  }

  onLeave(chatId: string) {
    console.log('Leaving chat:', chatId);
    this.activeChatId = null;
  }

  onViewInfo(chatId: string) {
    console.log('Viewing info for chat:', chatId);
    this.activeChatId = null;
  }


  // onDeleteChat(chatId: string, event: Event): void {
  //   event.stopPropagation();
  //   if (confirm('Are you sure you want to delete this chat?')) {
  //     this.chatService.deleteChat(chatId).subscribe({
  //       next: () => {
  //         this.chats = this.chats.filter(chat => chat.chatId !== chatId);
  //         console.log(`Chat ${chatId} deleted successfully`);
  //       },
  //       error: (error) => {
  //         console.error('Error deleting chat', error);
  //       }
  //     });
  //   }
  // }

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
