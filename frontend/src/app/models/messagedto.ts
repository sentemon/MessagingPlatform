import { UserDto } from "./userdto";

export interface MessageDto {
  id: string;
  senderId: string;
  sender: UserDto;
  chatId: string;
  content: string;
  sentAt: Date;
  updatedAt: Date | null;
  isRead: boolean;
}
