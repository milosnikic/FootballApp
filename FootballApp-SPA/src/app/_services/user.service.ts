import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl: string = 'http://localhost:5000/api/users';

  constructor(private http: HttpClient,
              private localStorage: LocalStorageService) {}

  getLatestFiveVisitors() {
    const params = new HttpParams();
    params.append('userId', JSON.parse(localStorage.getItem('user')).id);
    const userId = JSON.parse(this.localStorage.get('user')).id;
    return this.http.get(this.baseUrl + '/visitors' + `?userId=${userId}`, { params });
  }
}
