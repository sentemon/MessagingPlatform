import { UserDto } from "./userdto";
import { MessageDto } from "./messagedto";
import { UserChatDto } from "./userchatdto";

export class ChatDto {
  constructor(
    public id?: string,
    public chatType?: number,
    public userChats?: UserChatDto[],
    public messages?: MessageDto[],
    public creatorId?: string,
    public creator?: UserDto,
    public title?: string
    ) { }
}
