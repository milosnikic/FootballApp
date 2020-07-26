import { Component, OnInit, ViewEncapsulation, Output, EventEmitter, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { NotifyService } from 'src/app/_services/notify.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css'],
})
export class FriendsListComponent implements OnInit {
  users: User[];

  constructor(
    private userService: UserService,
    private notifyService: NotifyService,
    private localStorage: LocalStorageService) { }

  ngOnInit() {
    this.userService.getAllExploreUsers(JSON.parse(this.localStorage.get('user')).id).subscribe(
      (res: User[]) => {
        this.users = res;
      },
      (err) => {
        this.notifyService.showError('Error loading users..');
      }
    );
  }

}
