import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FriendRequestStatus } from "src/app/constants";
import { User } from "src/app/_models/user";
import { FriendsService } from "src/app/_services/friends.service";
import { LocalStorageService } from "src/app/_services/local-storage.service";
import { NotifyService } from "src/app/_services/notify.service";

@Component({
  selector: "app-friend-detail",
  templateUrl: "./friend-detail.component.html",
  styleUrls: ["./friend-detail.component.css"],
})
export class FriendDetailComponent implements OnInit {
  @Input()
  buttonDisplay: FriendRequestStatus;
  @Input() user: User;

  FriendRequestStatus = FriendRequestStatus;

  constructor(
    private friendsService: FriendsService,
    private localStorage: LocalStorageService,
    private notifyService: NotifyService
  ) {}

  ngOnInit() {}

  public addFriend(): void {
    this.friendsService
      .sendFriendRequest({
        senderId: this.localStorage.getCurrentLoggedInUserId(),
        receiverId: this.user.id,
      })
      .subscribe((res) => {
        if (res) {
          this.notifyService.showSuccess(res.value);
          this.buttonDisplay = FriendRequestStatus.Sent;
          this.refreshUsersLists();
        } else {
          this.notifyService.showError(res.value);
        }
      });
  }

  public remove(): void {
    this.friendsService
      .deleteFriendRequest({
        senderId: this.localStorage.getCurrentLoggedInUserId(),
        receiverId: this.user.id,
      })
      .subscribe((res) => {
        if (res) {
          this.buttonDisplay = FriendRequestStatus.NotSent;
          this.notifyService.showSuccess(res.value);
          this.refreshUsersLists();
        } else {
          this.notifyService.showError(res.value);
        }
      });
  }

  public acceptFriend(): void {
    this.friendsService
      .acceptFriendRequest({
        receiverId: this.localStorage.getCurrentLoggedInUserId(),
        senderId: this.user.id,
      })
      .subscribe((res) => {
        if (res) {
          this.buttonDisplay = FriendRequestStatus.Accepted;
          this.notifyService.showSuccess(res.value);
          this.refreshUsersLists();
        } else {
          this.notifyService.showError(res.value);
        }
      });
  }
  private refreshUsersLists(): void {
    this.friendsService.getAllExploreUsers(
      this.localStorage.getCurrentLoggedInUserId()
    );
    this.friendsService.pendingFriendRequests(
      this.localStorage.getCurrentLoggedInUserId()
    );
    this.friendsService.getAllFriendsForUser(
      this.localStorage.getCurrentLoggedInUserId()
    );
    this.friendsService.sentFriendRequests(
      this.localStorage.getCurrentLoggedInUserId()
    );
  }
}
