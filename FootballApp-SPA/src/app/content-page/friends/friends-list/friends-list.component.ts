import {
  Component,
  OnInit,
  Input,
} from "@angular/core";
import { User } from "src/app/_models/user";
import { NotifyService } from "src/app/_services/notify.service";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { FriendsService } from "src/app/_services/friends.service";
import { FriendRequestStatus } from "src/app/constants";

@Component({
  selector: "app-friends-list",
  templateUrl: "./friends-list.component.html",
  styleUrls: ["./friends-list.component.css"],
})
export class FriendsListComponent implements OnInit {
  @Input()
  users: User[];
  @Input()
  buttonDisplay: FriendRequestStatus;

  constructor() {}

  ngOnInit() {}
}
