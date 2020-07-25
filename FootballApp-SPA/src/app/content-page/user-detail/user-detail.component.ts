import { User } from './../../_models/user';
import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute, Data, ParamMap, Router } from '@angular/router';
import { MatTabGroup } from '@angular/material';
import { UserService } from 'src/app/_services/user.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class UserDetailComponent implements OnInit {
  user: User;
  userId: number;
  titleToDisplay: string;
  editable: boolean;

  @ViewChild('tabs', { static: true }) tabs: MatTabGroup;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private localStorage: LocalStorageService,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((res: ParamMap) => {
      if (res.get('id')) {
        this.userId = +res.get('id');
        if (this.userId === JSON.parse(this.localStorage.get('user')).id) {
          return this.router.navigate(['app/dashboard']);
        }
      } else {
        this.userId = JSON.parse(this.localStorage.get('user')).id;
      }
    });
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
      this.editable = data['editable'];
      // console.log(data['title']);
    });
    this.userService.getUserData(this.userId).subscribe((res: User) => {
      this.user = res;
      console.log(this.user);
    });
  }

  changeTabToEditProfile(event) {
    this.tabs.selectedIndex = event;
  }
}
