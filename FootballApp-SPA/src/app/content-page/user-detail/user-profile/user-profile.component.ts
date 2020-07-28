import { User } from './../../../_models/user';
import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { Visitor } from 'src/app/_models/visitor';
import { UserService } from 'src/app/_services/user.service';
import { Achievement } from 'src/app/_models/achievement';

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
  userAchievements: Achievement[] = [];
  achievements: Achievement[];

  constructor(private userService: UserService) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.userService.getUserData(this.userId).subscribe((res: User) => {
      this.user = res;
      this.userService.getLatestFiveVisitorsForUser(this.user.id).subscribe(
        (res: Visitor[]) => {
          this.visitors = res;
        },
        (err) => {}
      );
      this.userService.getAllAchievements().subscribe(
        (res: Achievement[]) => {
          this.achievements = res;
        },
        (err) => console.log(err)
      );
      // TODO: implement get achievements
      this.userService.getAchievementsForUser(this.userId).subscribe(
        (res: Achievement[]) => {
          this.userAchievements = res;
        },
        (err) => console.log(err)
      );
    });
  }

  ngOnInit() {}

  onEditProfile() {
    this.editProfile.emit(5);
  }

  getClass(achievement: Achievement) {
    for (let index = 0; index < this.userAchievements.length; index++) {
      const element = this.userAchievements[index];
      if(element.value === achievement.value) {
        return false; 
      }
    }
    return true;
  }
}
