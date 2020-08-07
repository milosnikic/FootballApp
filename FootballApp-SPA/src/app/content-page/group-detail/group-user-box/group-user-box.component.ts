import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { MatchStatus } from 'src/app/_models/matchStatus.enum';

@Component({
  selector: 'app-group-user-box',
  templateUrl: './group-user-box.component.html',
  styleUrls: ['./group-user-box.component.css']
})
export class GroupUserBoxComponent implements OnInit {
  @Input() user: any;
  @Input() deleteVisible: boolean = false;
  @Input() pendingRequest: boolean = false;
  @Input() matchStatus: MatchStatus;
  MatchStatus = MatchStatus;

  constructor() { }


  ngOnInit() {
  }

}
