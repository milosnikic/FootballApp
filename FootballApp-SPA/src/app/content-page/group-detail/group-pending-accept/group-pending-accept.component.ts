import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-group-pending-accept',
  templateUrl: './group-pending-accept.component.html',
  styleUrls: ['./group-pending-accept.component.css']
})
export class GroupPendingAcceptComponent implements OnInit {
  @Input()
  pendingRequests: User[];


  constructor() { }

  ngOnInit() {
  }

  filterUsers(userId) {
    this.pendingRequests = this.pendingRequests.filter(u => u.id !== userId);
  }

}
