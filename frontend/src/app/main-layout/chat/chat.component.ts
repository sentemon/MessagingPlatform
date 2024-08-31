import { Component, OnInit } from '@angular/core';
import { ChatService } from "../../services/chat/chat.service";
import { FormsModule } from "@angular/forms";
import { DatePipe, NgForOf, NgIf } from "@angular/common";
import { ChatDto } from "../../models/chatdto";
import { MessageDto } from "../../models/messagedto";

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
export class ChatComponent implements OnInit {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  selectedChatId: string = "b123c86f-b777-4431-a749-ffafd1d12b85"; // example id

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.loadChat();
  }

  loadChat(): void {
    this.chatService.getChat(this.selectedChatId).subscribe({
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
