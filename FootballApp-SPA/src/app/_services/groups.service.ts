import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root',
})
export class GroupsService {
  baseUrl: string = 'http://localhost:5000/api/groups';
  private allGroups = new Subject<Group[]>();
  private usersGroups = new Subject<Group[]>();
  private usersFavoriteGroups = new Subject<Group[]>();
  private usersCreatedGroups = new Subject<Group[]>();

  readonly allGroups$ = this.allGroups.asObservable();
  readonly usersGroups$ = this.usersGroups.asObservable();
  readonly usersFavoriteGroups$ = this.usersFavoriteGroups.asObservable();
  readonly usersCreatedGroups$ = this.usersCreatedGroups.asObservable();

  constructor(private http: HttpClient) {}

  createGroup(userId: number, data: any) {
    return this.http.post(this.baseUrl + `?userId=${userId}`, data);
  }

  getAllGroups() {
    return this.http.get(this.baseUrl + '/all').subscribe(
      (res: Group[]) => {
        this.allGroups.next(res);
      }
    );
  }

  getUsersGroups(userId: number) {
    return this.http.get(this.baseUrl + `?userId=${userId}`).subscribe(
      (res: any) => {
        this.usersGroups.next(res);
      }
    );
  }
}
