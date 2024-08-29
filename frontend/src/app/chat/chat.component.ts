import { Component, OnInit } from '@angular/core';
import { ChatService } from "../services/chat/chat.service";
import { FormsModule } from "@angular/forms";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import { ChatDto } from "../models/chatdto";

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
  selectedChatId: string = "1a6c68d2-26b8-48f0-a738-833d4fd4ace3"; // 1a6c68d2-26b8-48f0-a738-833d4fd4ace3

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.loadChat();
  }

  loadChat(): void {
    this.chatService.getChat(this.selectedChatId).subscribe({
      next: (data: ChatDto) => {
        this.chat = data;
      },
      error: (error) => {
        console.error('Error loading chat', error);
      }
    });
  }

}
