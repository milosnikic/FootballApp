import { Group } from './../../../_models/group';
import { Component, OnInit, Input } from '@angular/core';
import { MembershipStatus } from 'src/app/_models/MembershipStatus.enum';
import { GroupsService } from 'src/app/_services/groups.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { NotifyService } from 'src/app/_services/notify.service';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit {
  @Input() groups: Group[] = [];
  @Input() mode: MembershipStatus;
  MembershipStatus = MembershipStatus;
  userId: number;

  constructor(
    private groupService: GroupsService,
    private localStorage: LocalStorageService,
    private notifyService: NotifyService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.userId = JSON.parse(this.localStorage.get('user')).id;
  }

  requestToJoin(groupId: number) {
    this.groupService.requestToJoin(this.userId, groupId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.groups.find((g) => g.id === groupId).membershipStatus =
            MembershipStatus.Sent;
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err);
      }
    );
  }

  makeFavorite(groupId: number) {
    this.groupService.makeFavorite(this.userId, groupId).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.groups.find((g) => g.id === groupId).favorite = true;
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err);
      }
    );
  }
  makeUnfavorite(groupId: number) {
    this.groupService.makeUnfavorite(this.userId, groupId, false).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess(res.value);
          this.groups.find((g) => g.id === groupId).favorite = false;
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err);
      }
    );
  }

  openDialog(groupId: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '350px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.groupService.leaveGroup(this.userId, groupId).subscribe(
          (res: any) => {
            if (res.key) {
              this.notifyService.showSuccess(res.value);
              this.groups = this.groups.filter(g => g.id !== groupId);
            } else {
              this.notifyService.showError(res.value);
            }
          },
          (err) => {
            this.notifyService.showError(err);
          }
        );
      }
    });
  }
}
