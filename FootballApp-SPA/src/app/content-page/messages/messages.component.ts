import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material";
import { ActivatedRoute, Data } from "@angular/router";
import { ChatType } from "src/app/constants";
import { CreateChatModalComponent } from "src/app/modals/create-chat-modal/create-chat-modal.component";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { MessagesService } from "src/app/_services/messages.service";
import { NotifyService } from "src/app/_services/notify.service";
import { SignalRService } from "src/app/_services/signal-r.service";

@Component({
  selector: "app-messages",
  templateUrl: "./messages.component.html",
  styleUrls: ["./messages.component.css"],
})
export class MessagesComponent implements OnInit {
  titleToDisplay: string;
  private userId: number;
  public chats: any = [];
  public messagesForSpecificChat: any = [];
  public recipientId: number = null;
  public chatId: number = null;

  constructor(
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private messagesService: MessagesService,
    private localStorageService: LocalStorageService,
    private notify: NotifyService,
    private chatService: SignalRService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data["title"];
    });

    this.userId = this.localStorageService.getCurrentLoggedInUserId();

    this.messagesService.getPrivateChats(this.userId).subscribe(
      (res) => {
        this.chats = res;
        this.mapChatsFields();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  public openDialog(): void {
    const dialogRef = this.dialog.open(CreateChatModalComponent, {
      height: "500px",
    });

    dialogRef.afterClosed().subscribe((result) => {
      // We should check if result is not false
      // and create a new chat
      if (result) {
        const userToChatWith = result.friends[0];
        this.messagesService.createPrivateChat(userToChatWith.id).subscribe(
          (res) => {
            if (res.key) {
              this.notify.showSuccess(res.value);
            }
          },
          (err) => {
            this.notify.showError(err);
          }
        );
      }
    });
  }

  private mapChatsFields() {
    if (this.chats) {
      for (let index = 0; index < this.chats.length; index++) {
        let element = this.chats[index];
        let user = element.users.filter((p) => p.id !== this.userId)[0];
        element.photo = user.mainPhoto;
        element.name = user.firstname + " " + user.lastname;
        element.gender = user.gender;
        element.lastMessage = user.lastMessage;
        element.recipientId = user.id;
      }
    }
  }

  public onChatClicked(chat: any) {
    this.recipientId = chat.recipientId;
    this.messagesForSpecificChat = chat.messages;
    this.chatId = chat.id;
    this.chatService.joinRoom(chat.id);
  }
}
