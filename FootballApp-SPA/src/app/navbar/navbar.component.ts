import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { FriendsService } from '../_services/friends.service';
import { LocalStorageService } from '../_services/local-storage.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  // TODO: Change logic to fetch groups and users
  //       do filter serach on server side (order by date, take 5)
  //       and display loading icon if its loading
  //       and add search page with all searched items
  //       https://itnext.io/using-angular-6-material-auto-complete-with-async-data-6d89501c4b79

  allUsers: User[] = [];
  filteredUsers$: Observable<User[]>;
  userControl = new FormControl();
  user: User;
  @ViewChild('searchBox', { static: true }) searchBox: ElementRef;

  constructor(
    private router: Router,
    private friendsService: FriendsService,
    private authService: AuthService,
    private localStorage: LocalStorageService
  ) {
    this.filteredUsers$ = this.userControl.valueChanges.pipe(
      startWith('.'),
      map((firstname) =>
        firstname ? this._filterUsers(firstname) : this.allUsers.slice()
      )
    );
  }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));
    this.friendsService.getAllExploreUsers(this.user.id);
  }
  onClear() {
    this.searchBox.nativeElement.value = '';
  }

  logout() {
    this.localStorage.clear();
    this.authService.resetUser();
    this.router.navigate(['']);
  }

  private _filterUsers(value: string): User[] {
    const filterValue = value.toLowerCase();

    return this.allUsers.filter(
      (user) =>
        user.firstname.toLowerCase().indexOf(filterValue) === 0 ||
        user.lastname.toLowerCase().indexOf(filterValue) === 0 ||
        user.username.toLowerCase().indexOf(filterValue) === 0
    );
  }

  goToProfile(id: number) {
    this.router.navigate(['app/users/', id]);
  }
}
