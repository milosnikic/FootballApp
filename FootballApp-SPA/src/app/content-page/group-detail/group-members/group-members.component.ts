import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { MembershipStatus } from 'src/app/_models/MembershipStatus.enum';
import { Role } from 'src/app/_models/role.enum';

@Component({
  selector: 'app-group-members',
  templateUrl: './group-members.component.html',
  styleUrls: ['./group-members.component.css']
})
export class GroupMembersComponent implements OnInit {
  @Input() groupMembers: User[];
  // Membership info has 3 fields
  //            favorite
  //            status
  //            role
  @Input() membershipInfo: any;
  Role = Role;
  selectedUser: any = null;

  constructor() { }

  ngOnInit() {
  }

  setSelectedUser(user: any) {
    this.selectedUser = user;
  }
}
