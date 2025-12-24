import { UserDto } from "./userdto";

export interface UserChatDto {
  userId: string;
  user: UserDto;
  chatId: string;
  joinedAt: Date;
}
