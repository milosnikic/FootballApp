import { SelectionModel } from "@angular/cdk/collections";
import { Component, OnInit, ViewChild, ViewChildren } from "@angular/core";
import {
  MatDialogRef,
  MatListOption,
  MatSelectionList,
} from "@angular/material";
import { ChatType } from "src/app/constants";
import { UserToChat } from "src/app/_models/userToChat";
import { MessagesService } from "src/app/_services/messages.service";

@Component({
  selector: "app-create-chat-modal",
  templateUrl: "./create-chat-modal.component.html",
  styleUrls: ["./create-chat-modal.component.scss"],
})
export class CreateChatModalComponent implements OnInit {
  type = ChatType.Private;
  ChatType = ChatType;
  friends: UserToChat[] = [];

  @ViewChildren("selection") selection;

  constructor(
    public dialogRef: MatDialogRef<CreateChatModalComponent>,
    private messagesService: MessagesService
  ) {}

  ngOnInit() {
    this.messagesService.getAvailableUsers().subscribe(
      (res: UserToChat[]) => {
        this.friends = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  onCancelClick() {
    this.dialogRef.close(false);
  }

  onCreateClick() {
    this.dialogRef.close({
      type: this.type as ChatType,
      friends: this.selection.last._value,
    });
  }
}
