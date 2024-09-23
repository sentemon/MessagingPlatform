import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ChatService } from '../../../services/chat/chat.service';
import { ChatDto } from '../../../models/chatdto';
import { MessageDto } from '../../../models/messagedto';
import { AddMessageDto } from '../../../models/addmessagedto';
import { UserDto } from '../../../models/userdto';
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {MessageService} from "../../../services/message/message.service";

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
export class ChatComponent implements OnChanges {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  newMessage: string = '';
  @Input() selectedChatId: string | null = null;

  constructor(private chatService: ChatService, private messageService: MessageService) {
    this.chatService.onReceiveMessage((user, message) => {
      const userDto: UserDto = { id: '', firstName: '', lastName: '', username: user, email: '', bio: '', isOnline: null, accountCreatedAt: new Date() };
      if (this.chat.id === this.selectedChatId) {
        this.messages.push({
          id: '',
          senderId: userDto.id,
          sender: userDto,
          chatId: this.chat.id || '',
          content: message,
          sentAt: new Date(),
          updatedAt: null,
          isRead: false
        });
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedChatId'] && this.selectedChatId) {
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
        chatId: this.chat.id || '',
        content: this.newMessage
      };

      this.messageService.addMessage(addMessageDto).subscribe({
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

  // ToDo: fix this method
  deleteMessage(senderId: string, messageId: string): void {
    console.log('Deleting message', senderId, messageId);
    this.messageService.deleteMessage(senderId, messageId).subscribe({
      next: () => {
        this.messages = this.messages.filter(m => m.id !== messageId);
      },
      error: (error) => {
        console.error('Error deleting message', error);
      }
    });
  }

  deleteChat(chatId: string): void {
    this.chatService.deleteChat(chatId).subscribe({
      next: () => {
        this.chat = new ChatDto();
        this.messages = [];
      },
      error: (error) => {
        console.error('Error deleting chat', error);
      }
    });
  }

}
