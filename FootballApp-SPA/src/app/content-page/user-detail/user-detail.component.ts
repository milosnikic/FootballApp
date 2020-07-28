import { User } from './../../_models/user';
import { Component, OnInit, ViewEncapsulation, ViewChild, OnChanges } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute, Data, ParamMap, Router } from '@angular/router';
import { MatTabGroup } from '@angular/material';
import { UserService } from 'src/app/_services/user.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { CommentsService } from 'src/app/_services/comments.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class UserDetailComponent implements OnInit{
  titleToDisplay: string;
  userId: number;
  editable: boolean;
  // TODO: add comments model
  comments: any[];

  @ViewChild('tabs', { static: true }) tabs: MatTabGroup;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private localStorage: LocalStorageService,
    private router: Router,
    private commentsService: CommentsService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((res: ParamMap) => {
      if (res.get('userId')) {
        // We are on apps/users/{userId} route
        this.userId = +res.get('userId');
        if (this.userId === JSON.parse(this.localStorage.get('user')).id) {
          // On route of own id
          return this.router.navigate(['app/dashboard']);
        }
        this.userService.visitUser(this.userId).subscribe();
      } else {
        // We are on dashboard route directly
        this.userId = JSON.parse(this.localStorage.get('user')).id;
      }
      this.commentsService.getCommentsForUser(this.userId).subscribe(
        (res: any) => {
          this.comments = res;
        },
        err => {
          console.log(err);
        }
      );
    });
    this.route.data.subscribe((data: Data) => {
      this.titleToDisplay = data['title'];
      this.editable = data['editable'];
    });
  }

  changeTab(event) {
    this.tabs.selectedIndex = event;
  }
}
