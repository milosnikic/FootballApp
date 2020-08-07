import { Component, OnInit } from '@angular/core';
import { MatchStatus } from 'src/app/_models/matchStatus.enum';

@Component({
  selector: 'app-upcoming-match',
  templateUrl: './upcoming-match.component.html',
  styleUrls: ['./upcoming-match.component.css'],
})
export class UpcomingMatchComponent implements OnInit {
  isChecked: boolean = false;
  isConfirmed: boolean = false;

  users: any = [
    {
      firstname: 'Milos',
      lastname: 'Nikic',
      username: 'kicni',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'milos.nikic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth: '31.12.1996.',
      matchStatus: MatchStatus.Checked
    },
    {
      firstname: 'jovan',
      lastname: 'jovanovic',
      username: 'joca',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'jovan.jovanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth: '31.12.1996.',
      matchStatus: MatchStatus.Checked
    },
    {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth: '31.12.1996.',
      matchStatus: MatchStatus.Checked
    },
    {
      firstname: 'Ivan',
      lastname: 'Ivanovic',
      username: 'ica',
      dateCreated: '1.1.2020.',
      mainPhoto: '../../../../assets/male-default.jpg',
      email: 'ivan.ivanovic@gmail.com',
      lastActive: '20.5.2020.',
      dateOfBirth: '31.12.1996.',
      matchStatus: MatchStatus.Confirmed
    },
  ];

  constructor() {}

  ngOnInit() {}

  checkIn() {
    this.isChecked = true;
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
      matchStatus: MatchStatus.Checked
    };
    this.users.unshift(user);
  }

  confirm() {
    this.isConfirmed = true;
    // TODO: implement saving state in db
  }

  giveUp() {
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
    if (this.users.length < 5) {
      return 'low-capacity';
    } else if (this.users.length < 8) {
      return 'moderate-capacity';
    } else if (this.users.length < 11) {
      return 'high-capacity';
    }
    return 'full-capacity';
  }
}
