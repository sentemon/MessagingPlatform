export class ChatSidebar {
  constructor(
    public chatId: string,
    public title: string,
    public unreadMessagesCount: number,
    public lastMessageFrom?: string,
    public lastMessageContent?: string,
    public lastMessageSentAt?: string,
    public avatarUrl?: string
  ) { }

}
