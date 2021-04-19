import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  Input,
  Output,
  EventEmitter,
} from "@angular/core";

@Component({
  selector: "app-messages-list-users",
  templateUrl: "./messages-list-users.component.html",
  styleUrls: ["./messages-list-users.component.css"],
})
export class MessagesListUsersComponent implements OnInit {
  @ViewChild("userButton", { static: true }) userButton: ElementRef;
  @Input()
  public chats: any[] = [];
  @Output() chatClicked = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  onChatClick(chat: any) {
    this.chatClicked.emit(chat);
  }
}
