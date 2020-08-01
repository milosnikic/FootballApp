import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { UserService } from '../_services/user.service';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  allUsers: User[] = [];
  filteredUsers$: Observable<User[]>;
  userControl = new FormControl();
  user: User;
  @ViewChild('searchBox', { static: true }) searchBox: ElementRef;

  constructor(private router: Router, private userService: UserService) {
    this.filteredUsers$ = this.userControl.valueChanges.pipe(
      startWith('.'),
      map((firstname) =>
        firstname ? this._filterUsers(firstname) : this.allUsers.slice()
      )
    );
  }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem('user'));
    this.userService.getAllExploreUsers(this.user.id).subscribe(
      (res: User[]) => {
        this.allUsers = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }
  onClear() {
    this.searchBox.nativeElement.value = '';
  }

  logout() {
    localStorage.clear();
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
