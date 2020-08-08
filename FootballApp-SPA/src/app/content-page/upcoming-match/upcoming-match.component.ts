import { Component, OnInit } from '@angular/core';
import { MatchStatus } from 'src/app/_models/matchStatus.enum';
import { MatchService } from 'src/app/_services/match.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: 'app-upcoming-match',
  templateUrl: './upcoming-match.component.html',
  styleUrls: ['./upcoming-match.component.css'],
})
export class UpcomingMatchComponent implements OnInit {
  isChecked: boolean = false;
  isConfirmed: boolean = false;

  users: any = [];
  match: any;
  matchId: number;

  constructor(private matchService: MatchService,
              private route: ActivatedRoute,
              private notifyService: NotifyService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(
      (data: ParamMap) => {
        this.matchId = +data.get('matchId');
        this.matchService.getUpcomingMatchInfo(this.matchId).subscribe(
          (res: any) => {
            this.match =  res;
            this.users = this.match.appliedUsers;
          }
        );
      }
    );

  }

  checkIn() {
    const user = JSON.parse(localStorage.getItem('user'));
    this.matchService.checkIn(user.id, this.matchId).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
          this.isChecked = true;
          user.matchStatus = MatchStatus.Checked;
          this.users.unshift(user);
        }else {
          this.notifyService.showError(res.value);
        }
      },
      err => {
        this.notifyService.showError(err.error);
      }
    );
  }

  confirm() {
    this.isConfirmed = true;
    // TODO: implement saving state in db
  }

  giveUp() {
    // TODO: implement this to call api
    this.isChecked = false;
    // const user = JSON.parse(localStorage.getItem('user'));
    const user = {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth: '31.12.1996.',
    };
    this.users = this.users.filter((u) => u.firstname !== user.firstname);
  }

  getCapacityClass() {
    if(this.users.length === 0) {
      return 'zero-capacity';
    } else if (this.users.length < 5) {
      return 'low-capacity';
    } else if (this.users.length < 8) {
      return 'moderate-capacity';
    } else if (this.users.length < 11) {
      return 'high-capacity';
    }
    return 'full-capacity';
  }
}
