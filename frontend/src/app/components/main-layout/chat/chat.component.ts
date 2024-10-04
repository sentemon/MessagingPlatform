import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ChatService } from '../../../services/chat/chat.service';
import { ChatDto } from '../../../models/chatdto';
import { MessageDto } from '../../../models/messagedto';
import { UserDto } from '../../../models/userdto';
import { DatePipe, NgForOf, NgIf, NgOptimizedImage } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { SignalrService } from "../../../services/signalr/signalr.service";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    DatePipe,
    FormsModule,
    NgForOf,
    NgIf,
    NgOptimizedImage
  ],
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnChanges {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  newMessage: string = '';

  @Input() selectedChatId: string | null = null;
  @Output() chatDeleted = new EventEmitter<string>();

  constructor(private chatService: ChatService, private signalrService: SignalrService) {
    this.signalrService.messageReceived.subscribe(({ sender, content }) => {
      const userDto: UserDto = { id: '', firstName: '', lastName: '', username: sender, email: '', bio: '', isOnline: null, accountCreatedAt: new Date() };
      if (this.chat.id === this.selectedChatId) {
        this.messages.push({
          id: '',
          senderId: userDto.id,
          sender: userDto,
          chatId: this.chat.id || '',
          content: content,
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
    this.chatService.get(chatId).subscribe({
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
      this.signalrService.sendMessage(this.chat.id || '', this.newMessage).subscribe({
        next: () => {
          this.messages.push({
            id: '',
            senderId: '',
            sender: { id: '', firstName: '', lastName: '' },
            chatId: this.chat.id || '',
            content: this.newMessage,
            sentAt: new Date(),
            updatedAt: null,
            isRead: false
          });
          this.newMessage = '';
        },
        error: (error: any) => {
          console.error('Error sending message', error);
        }
      });
    }
  }

  // deleteChat(chatId: string): void {
  //   if (confirm('Are you sure you want to delete this chat?')) {
  //     this.chatService.delete(chatId).subscribe({
  //       next: () => {
  //         this.chat = new ChatDto();
  //         this.messages = [];
  //         this.chatDeleted.emit(chatId);
  //       },
  //       error: (error) => {
  //         console.error('Error deleting chat', error);
  //       }
  //     });
  //   }
  // }
}
