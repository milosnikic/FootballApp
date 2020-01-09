import { User } from './../../_models/user';
import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute, Data } from '@angular/router';
import { MatTabGroup } from '@angular/material';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class UserDetailComponent implements OnInit {
  user: User = {
    username: 'kicni',
    firstname: 'Milos',
    lastname: 'Nikic',
    email: 'milos.nikic@gmail.com',
    dateOfBirth: new Date('1996-12-31T00:00:00'),
    lastActive: new Date('0001-01-01T00:00:00'),
    created: new Date('2019-12-17T12:48:12.1652862'),
    gender: 'male',
    city: 'Beograd',
    country: 'Srbija'
    // memberships: []
  };

  titleToDisplay: string;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
      // console.log(data['title']);
    });
  }
}
