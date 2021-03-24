import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Data } from "@angular/router";
import { User } from "src/app/_models/user";
import { FriendsService } from "src/app/_services/friends.service";
import { FriendRequestStatus } from "src/app/constants";
import { Observable } from "rxjs";
import { LocalStorageService } from "src/app/_services/local-storage.service";

@Component({
  selector: "app-friends",
  templateUrl: "./friends.component.html",
  styleUrls: ["./friends.component.css"],
  encapsulation: ViewEncapsulation.None,
})
export class FriendsComponent implements OnInit {
  userId: number;
  titleToDisplay: string;
  displayUserDetail: boolean = false;
  exploreUsers$: Observable<User[]>;
  friends$: Observable<User[]>;
  pendingRequests$: Observable<User[]>;
  sentFriendRequests$: Observable<User[]>;
  FriendRequestStatus = FriendRequestStatus;

  constructor(
    private route: ActivatedRoute,
    private friendsService: FriendsService,
    private localStorage: LocalStorageService
  ) {}

  ngOnInit() {
    this.userId = this.localStorage.getCurrentLoggedInUserId();
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data["title"];
    });

    this.friendsService.getAllExploreUsers(this.userId);
    this.friendsService.pendingFriendRequests(this.userId);
    this.friendsService.getAllFriendsForUser(this.userId);
    this.friendsService.sentFriendRequests(this.userId);

    this.exploreUsers$ = this.friendsService.exploreUsers$;
    this.friends$ = this.friendsService.friends$;
    this.sentFriendRequests$ = this.friendsService.sentFriendRequests$;
    this.pendingRequests$ = this.friendsService.pendingRequests$;
  }

  showUserDetails(event) {
    this.displayUserDetail = event;
  }
}
