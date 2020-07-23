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
  user: User = JSON.parse(localStorage.getItem('user'));

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
