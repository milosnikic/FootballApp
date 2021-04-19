import { UserToChat } from "./userToChat";

export class Message {
  id: number;
  sender: UserToChat;
  content: string;
  messageSent: Date;
}
