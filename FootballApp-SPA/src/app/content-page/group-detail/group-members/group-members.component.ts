import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-group-members',
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.css']
})
export class GroupMembersComponent implements OnInit {
  @Input() groupMembers: User[];
  selectedUser: any = null;

  constructor() { }

  ngOnInit() {
  }

  setSelectedUser(user: any) {
    this.selectedUser = user;
  }
}
