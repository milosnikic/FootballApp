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
    age: 23,
    lastActive: new Date('0001-01-01T00:00:00'),
    created: new Date('2019-12-17T12:48:12.1652862'),
    gender: 'male',
    city: 'Beograd',
    country: 'Srbija',
    photos: [
      {
        id: 1,
        path:
          'https://images.unsplash.com/photo-1503023345310-bd7c1de61c7d?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&w=1000&q=80',
        description: 'It student at Faculty of Organizational Sciences',
        dateAdded: new Date(),
        isMain: true
      }
    ]
    // memberships: []
  };

  titleToDisplay: string;
  editable: boolean;
  

  @ViewChild('tabs', { static: true }) tabs: MatTabGroup;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
      this.editable = data['editable'];
      // console.log(data['title']);
    });
  }

  changeTabToEditProfile(event) {
    this.tabs.selectedIndex = event;
  }
}
