import { Component, OnInit, Input } from '@angular/core';
import { Group } from 'src/app/_models/group';
import { GroupsService } from 'src/app/_services/groups.service';
import { MembershipStatus } from 'src/app/_models/MembershipStatus.enum';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: 'app-group-about',
  templateUrl: './group-about.component.html',
  styleUrls: ['./group-about.component.css']
})
export class GroupAboutComponent implements OnInit {
  @Input() group: any;
  @Input() user: any;
  // TODO: add membership info
  @Input() membershipInfo: any;
  MembershipStatus = MembershipStatus;

  constructor(private groupService: GroupsService,
              private notifyService: NotifyService) { }

  ngOnInit() {
  }

  requestJoin() {
    this.groupService.requestToJoin(this.user.id, this.group.id)
      .subscribe(
        (res: any) => {
          if(res.key) {
            this.notifyService.showSuccess(res.value);
            this.membershipInfo = {
              membershipStatus: MembershipStatus.Sent
            };
          }else {
            this.notifyService.showError(res.value);
          }
        },
        err => {
          this.notifyService.showError(err.error);
        }
      );
  }

  leaveGroup() {
    this.groupService.leaveGroup(this.user.id, this.group.id).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
          this.membershipInfo.membershipStatus = MembershipStatus.NotMember;
        }
      },
      err => {
        this.notifyService.showError(err.error);
      }
    );
  }

  favoriteGroup() {
    this.groupService.makeFavorite(this.user.id, this.group.id).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
          this.membershipInfo.favorite = true;
        }
      },
      err => {
        this.notifyService.showError(err.error);
      }
    );
  }
  
  unfavoriteGroup() {
    this.groupService.makeUnfavorite(this.user.id, this.group.id, false).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
          this.membershipInfo.favorite = false;
        }
      },
      err => {
        this.notifyService.showError(err.error);
      }
    );
  }
}
