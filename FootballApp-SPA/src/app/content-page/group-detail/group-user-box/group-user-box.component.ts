import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/_models/user';
import { MatchStatus } from 'src/app/_models/matchStatus.enum';
import { GroupsService } from 'src/app/_services/groups.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: 'app-group-user-box',
  templateUrl: './group-user-box.component.html',
  styleUrls: ['./group-user-box.component.css'],
})
export class GroupUserBoxComponent implements OnInit {
  groupId: number;
  @Output() userId = new EventEmitter<number>();
  @Input() user: any;
  @Input() deleteVisible: boolean = false;
  @Input() pendingRequest: boolean = false;
  @Input() matchStatus: MatchStatus;
  MatchStatus = MatchStatus;

  constructor(
    private groupService: GroupsService,
    private route: ActivatedRoute,
    private notifyService: NotifyService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((data: ParamMap) => {
      this.groupId = +data.get('groupId');
    });
  }

  acceptUser(user) {
    this.groupService.acceptUser(user.id, this.groupId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.userId.emit(user.id);
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err.error);
      }
    );
  }

  rejectUser(user) {
    this.groupService.rejectUser(user.id, this.groupId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.userId.emit(user.id);
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err.error);
      }
    );
  }
}
