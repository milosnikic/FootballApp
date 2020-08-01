import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { NotifyService } from 'src/app/_services/notify.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FriendsComponent implements OnInit {
  titleToDisplay: string;
  displayUserDetail: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit() {
    const userId = JSON.parse(localStorage.getItem('user')).id;
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
    });
  }

  showUserDetails(event) {
    this.displayUserDetail = event;
  }
}
