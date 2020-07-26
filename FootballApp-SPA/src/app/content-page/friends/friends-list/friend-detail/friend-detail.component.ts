import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-friend-detail',
  templateUrl: './friend-detail.component.html',
  styleUrls: ['./friend-detail.component.css']
})
export class FriendDetailComponent implements OnInit {
  @Input()
  buttonDisplay: string;
  @Input() user: User;
  constructor() {}

  ngOnInit() {
  }

  addFriend(){
    this.buttonDisplay = 'requestSent';
  }


  remove() {
    this.buttonDisplay = 'addFriend';
  }
}
