import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-messages-list-users',
  templateUrl: './messages-list-users.component.html',
  styleUrls: ['./messages-list-users.component.css']
})
export class MessagesListUsersComponent implements OnInit {
  @ViewChild('userButton', { static: true }) userButton: ElementRef;
  constructor() {}

  ngOnInit() {}

  onClick() {
    console.log(this.userButton);
  }
}
