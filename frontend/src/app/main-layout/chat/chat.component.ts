import {Component, Input, OnChanges, OnInit} from '@angular/core';
import { ChatService } from '../../services/chat/chat.service';
import { ChatDto } from '../../models/chatdto';
import { FormsModule } from '@angular/forms';
import { MessageDto } from '../../models/messagedto';
import { UserDto } from '../../models/userdto';
import { AddMessageDto } from '../../models/addmessagedto';
import {DatePipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    DatePipe,
    FormsModule,
    NgForOf,
    NgIf
  ],
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit, OnChanges {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  newMessage: string = '';
  @Input() selectedChatId: string | null = null;

  constructor(private chatService: ChatService) {
    this.chatService.onReceiveMessage((user, message) => {
      const userDto: UserDto = { id: '', firstName: '', lastName: '', username: user, email: '', bio: '', isOnline: null, accountCreatedAt: new Date() };
      this.messages.push({
        id: '',
        senderId: userDto.id,
        sender: userDto,
        chatId: this.chat.id!,
        content: message,
        sentAt: new Date(),
        updatedAt: null,
        isRead: false
      });
    });
  }

  ngOnInit(): void {
    if (this.selectedChatId) {
      this.loadChat(this.selectedChatId);
    }
  }

  ngOnChanges(): void {
    if (this.selectedChatId) {
      this.loadChat(this.selectedChatId);
    }
  }

  loadChat(chatId: string): void {
    this.chatService.getChat(chatId).subscribe({
      next: (data: ChatDto) => {
        this.chat = data;

        if (this.chat.messages && (this.chat.messages as any).$values) {
          const messageRefs = (this.chat.messages as any).$values as MessageDto[];
          this.messages = messageRefs || [];
        } else {
          this.messages = [];
        }
      },
      error: (error) => {
        console.error('Error loading chat', error);
      }
    });
  }

  sendMessage(): void {
    if (this.chat && this.newMessage) {
      const addMessageDto: AddMessageDto = {
        chatId: this.chat.id!,
        content: this.newMessage
      };

      this.chatService.addMessage(addMessageDto).subscribe({
        next: (response: MessageDto) => {
          this.messages.push({
            id: response.id,
            senderId: response.senderId,
            sender: response.sender,
            chatId: response.chatId,
            content: this.newMessage,
            sentAt: new Date(response.sentAt),
            updatedAt: response.updatedAt ? new Date(response.updatedAt) : null,
            isRead: response.isRead
          });
          this.newMessage = '';
        },
        error: (error: any) => {
          console.error('Error sending message', error);
        }
      });
    }
  }

}
