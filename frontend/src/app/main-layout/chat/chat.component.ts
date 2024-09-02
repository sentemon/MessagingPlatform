import {Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {ChatService} from "../../services/chat/chat.service";
import {FormsModule} from "@angular/forms";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {ChatDto} from "../../models/chatdto";
import {MessageDto} from "../../models/messagedto";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: './chat.component.html',
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    DatePipe
  ],
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnChanges {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  @Input() selectedChatId: string | null = null;

  constructor(private chatService: ChatService) {
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
}
