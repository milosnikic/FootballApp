import { User } from './../../../_models/user';
import { Component, OnInit, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Visitor } from 'src/app/_models/visitor';
import { UserService } from 'src/app/_services/user.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
})
export class UserProfileComponent implements OnInit, OnChanges {
  user: User;
  @Input() userId: number;
  @Input() editable: boolean;
  @Output() editProfile = new EventEmitter<number>();
  visitors: Visitor[];

  constructor(private userService: UserService) {}
  ngOnChanges(changes: SimpleChanges): void {
    this.userService.getUserData(this.userId).subscribe((res: User) => {
      this.user = res;
      this.userService.getLatestFiveVisitorsForUser(this.user.id).subscribe(
        (res: Visitor[]) => {
          this.visitors = res;
        },
        (err) => {
        }
      );
    });
  }

  ngOnInit() {
  }



  onEditProfile() {
    this.editProfile.emit(5);
  }
}
