import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ChatService } from '../../../services/chat/chat.service';
import { ChatDto } from '../../../models/responses/chatdto';
import { MessageDto } from '../../../models/responses/messagedto';
import { UserDto } from '../../../models/responses/userdto';
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
export class ChatComponent implements OnInit, OnChanges {
  chat: ChatDto = new ChatDto();
  messages: MessageDto[] = [];
  newMessage: string = '';
  private userById = new Map<string, UserDto>();
  private userByRef = new Map<string, UserDto>();
  private messageByRef = new Map<string, any>();

  @Input() selectedChatId: string | null = null;
  @Output() chatDeleted = new EventEmitter<string>();

  constructor(private chatService: ChatService, private signalrService: SignalrService) {
    this.signalrService.messageReceived.subscribe(({ chatId, sender, content, sentAt, updatedAt, senderMeta }) => {
      if (!this.chat.id || !chatId) return;
      if (chatId.toLowerCase() !== (this.selectedChatId || '').toLowerCase()) return;
      const message = this.normalizeMessage({
        chatId,
        content,
        sentAt,
        updatedAt,
        senderId: senderMeta?.id ?? '',
        sender: {
          id: senderMeta?.id ?? '',
          firstName: senderMeta?.firstName ?? '',
          lastName: senderMeta?.lastName ?? '',
          username: senderMeta?.username ?? sender,
          email: '',
          bio: '',
          isOnline: null,
          accountCreatedAt: new Date()
        }
      });
      this.messages.push(message);
    });
  }

  ngOnInit(): void {
    this.signalrService.connect().catch(err => console.error('SignalR connect failed', err));
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedChatId'] && this.selectedChatId) {
      this.loadChat(this.selectedChatId);
    }
  }

  loadChat(chatId: string): void {
    this.chatService.get(chatId).subscribe({
      next: (data: ChatDto) => {
        this.chat = this.normalizeChat(data);
        this.buildUserIndex(this.chat);
        this.buildMessageIndex(this.chat);

        if (this.chat.messages && (this.chat.messages as any).$values) {
          const messageRefs = (this.chat.messages as any).$values as any[];
          this.messages = (messageRefs || []).map((m) => this.normalizeMessage(this.resolveMessage(m)));
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
    const content = this.newMessage?.trim();
    if (!this.chat || !content) return;

    this.signalrService.sendMessage(this.chat.id || '', content).subscribe({
      next: () => {
        this.newMessage = '';
      },
      error: (error: any) => {
        console.error('Error sending message', error);
      }
    });
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

  private normalizeChat(chat: any): ChatDto {
    return {
      id: chat.id ?? chat.Id,
      chatType: chat.chatType ?? chat.ChatType,
      userChats: chat.userChats ?? chat.UserChats,
      messages: chat.messages ?? chat.Messages,
      creatorId: chat.creatorId ?? chat.CreatorId,
      creator: this.normalizeUser(chat.creator ?? chat.Creator),
      title: chat.title ?? chat.Title
    } as ChatDto;
  }

  private buildUserIndex(chat: ChatDto): void {
    this.userById.clear();
    this.userByRef.clear();
    this.messageByRef.clear();

    const userChats: any[] = (chat.userChats as any)?.$values ?? chat.userChats ?? [];
    userChats.forEach((uc: any) => {
      const userRaw = uc.user ?? uc.User;
      const userDto = this.normalizeUser(userRaw);
      if (!userDto) return;
      if (userDto.id) this.userById.set(userDto.id, userDto);
      const refId = userRaw?.$id ?? userRaw?.$Id;
      if (refId) this.userByRef.set(refId, userDto);

      // index messages under each user (full payload objects with $id)
      const userMessages: any[] = userRaw?.messages?.$values ?? userRaw?.Messages?.$values ?? [];
      userMessages.forEach(msg => {
        const refKey = msg?.$id ?? msg?.$Id;
        if (refKey) this.messageByRef.set(refKey, msg);
      });
    });
  }

  private buildMessageIndex(chat: ChatDto): void {
    // also index messages from chat.messages if they have $id
    const chatMessages: any[] = (chat.messages as any)?.$values ?? chat.messages ?? [];
    chatMessages.forEach(msg => {
      const refKey = msg?.$id ?? msg?.$Id;
      if (refKey) this.messageByRef.set(refKey, msg);
    });
  }

  private resolveMessage(raw: any): any {
    if (!raw) return raw;
    const refKey = raw.$ref ?? raw.$Ref;
    if (refKey && this.messageByRef.has(refKey)) {
      return this.messageByRef.get(refKey);
    }
    return raw;
  }

  private normalizeMessage(message: any): MessageDto {
    const rawSender = message.sender ?? message.Sender;
    let sender = this.normalizeUser(rawSender);
    const senderFullName = message.senderFullName ?? message.SenderFullName ?? '';
    // if sender is missing but full name exists, synthesize a user dto
    const synthesizedSender = !sender && senderFullName
      ? {
          id: '',
          firstName: senderFullName.split(' ')[0] ?? '',
          lastName: senderFullName.split(' ').slice(1).join(' ') ?? '',
          username: senderFullName,
          email: '',
          bio: '',
          isOnline: null,
          accountCreatedAt: new Date()
        }
      : sender;

    // resolve $ref or lookup by senderId
    if (!sender || (rawSender && rawSender.$ref)) {
      const refKey = rawSender?.$ref ?? rawSender?.$Ref;
      if (refKey && this.userByRef.has(refKey)) {
        sender = this.userByRef.get(refKey)!;
      } else {
        const sid = message.senderId ?? message.SenderId;
        if (sid && this.userById.has(sid)) {
          sender = this.userById.get(sid)!;
        }
      }
    }

    const sentAtRaw = message.sentAt ?? message.SentAt ?? new Date();
    const updatedRaw = message.updatedAt ?? message.UpdatedAt ?? null;
    return {
      id: message.id ?? message.Id ?? '',
      senderId: message.senderId ?? message.SenderId ?? sender?.id ?? synthesizedSender?.id ?? '',
      sender: sender ?? synthesizedSender,
      chatId: message.chatId ?? message.ChatId ?? '',
      content: message.content ?? message.Content ?? '',
      sentAt: new Date(sentAtRaw),
      updatedAt: updatedRaw ? new Date(updatedRaw) : null,
      isRead: message.isRead ?? message.IsRead ?? false
    } as MessageDto;
  }

  getSenderLabel(message: MessageDto): string {
    const first = message.sender?.firstName?.trim();
    const last = message.sender?.lastName?.trim();
    const username = message.sender?.username?.trim();

    const full = [first, last].filter(Boolean).join(' ').trim();
    if (full) return full;
    if (username) return username;
    return 'Unknown';
  }

  private normalizeUser(user: any): UserDto | null {
    if (!user) return null;
    return {
      id: user.id ?? user.Id ?? '',
      firstName: user.firstName ?? user.FirstName ?? '',
      lastName: user.lastName ?? user.LastName ?? '',
      username: user.username ?? user.Username ?? '',
      email: user.email ?? user.Email ?? '',
      bio: user.bio ?? user.Bio ?? '',
      isOnline: user.isOnline ?? user.IsOnline ?? null,
      accountCreatedAt: user.accountCreatedAt ? new Date(user.accountCreatedAt) : (user.AccountCreatedAt ? new Date(user.AccountCreatedAt) : new Date())
    } as UserDto;
  }
}
