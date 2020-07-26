import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl: string = 'http://localhost:5000/api/users';

  constructor(
    private http: HttpClient,
    private localStorage: LocalStorageService
  ) {}

  getLatestFiveVisitorsForUser(userId: number) {
    return this.http.get(this.baseUrl + '/visitors' + `?userId=${userId}`);
  }
  
  getUserData(userId: number) {
    return this.http.get(this.baseUrl + `/${userId}`);
  }

  visitUser(visitedId: number) {
    const data = {
      visitorId: JSON.parse(this.localStorage.get('user')).id,
      visitedId,
      dateVisited: new Date().toLocaleString()
    };

    return this.http.post(this.baseUrl + '/visit', data);
  }

  getAllExploreUsers(userId: number) {
    return this.http.get(this.baseUrl + '/explore' + `?userId=${userId}`);
  }
}
