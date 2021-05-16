import { Component, ElementRef, Input, OnInit, ViewChild } from "@angular/core";
import { Message } from "src/app/_models/message";
import { User } from "src/app/_models/user";
import { UserToChat } from "src/app/_models/userToChat";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { NotifyService } from "src/app/_services/notify.service";
import { SignalRService } from "src/app/_services/signal-r.service";

@Component({
  selector: "app-messages-display-content",
  templateUrl: "./messages-display-content.component.html",
  styleUrls: ["./messages-display-content.component.css"],
})
export class MessagesDisplayContentComponent implements OnInit {
  @Input()
  messages: Message[] = [];
  messageContent: string;
  @Input()
  recipientId: number = null;
  @Input()
  chatId: number = null;
  public sender: UserToChat;

  @ViewChild("chatScroll", { static: false })
  private scrollContainer: ElementRef;

  constructor(
    private localStorageService: LocalStorageService,
    private chatService: SignalRService,
    private notify: NotifyService
  ) {}

  ngOnInit() {
    if (this.messages.length > 0) {
      this.scrollToBottom();
    }
    this.sender = this.getChatSenderFromCurrentlyLoggedUser();
    this.chatService.messageReceived$.subscribe((res: Message) => {
      if (res.sender.id !== this.sender.id) {
        this.messages.push(
          this.createNewMessage(res.content, res.sender, res.messageSent)
        );
      }
    });
  }

  ngAfterViewChecked() {
    if (this.messages.length > 0) {
      this.scrollToBottom();
    }
  }

  private getChatSenderFromCurrentlyLoggedUser(): UserToChat {
    var user: User = JSON.parse(this.localStorageService.get("user"));
    var userToChat = this.mapUserToChatUser(user);
    return userToChat;
  }
  private mapUserToChatUser(user: User): UserToChat {
    var userToChat = new UserToChat();
    userToChat.firstname = user.firstname;
    userToChat.lastname = user.lastname;
    userToChat.gender = user.gender;
    userToChat.id = user.id;
    userToChat.mainPhoto = user.photos.filter((p) => p.isMain)[0] ? user.photos.filter((p) => p.isMain)[0].image : null;
    userToChat.username = user.username;
    return userToChat;
  }

  public sendMessage() {
    if (this.messageContent) {
      this.chatService.sendMessage(this.messageContent, this.chatId).subscribe(
        (res) => {
          if (res.key) {
            this.messages.push(
              this.createNewMessage(this.messageContent, this.sender, new Date())
            );
            this.messageContent = "";
            this.scrollToBottom();
          }
        },
        (err) => {
          this.notify.showError("Error sending message");
        }
      );
    }
  }
  private createNewMessage(
    content: string,
    sender: UserToChat,
    messageSent: Date
  ): Message {
    var message = new Message();
    message.content = content;
    message.sender = sender;
    message.messageSent = messageSent;
    return message;
  }

  private scrollToBottom() {
    try {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    } catch (error) {
      console.log(`Scroll error: ${error}`);
    }
  }
}
