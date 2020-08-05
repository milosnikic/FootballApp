import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { LocalStorageService } from './local-storage.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // TODO: Anytime in future
  //       Refactor code to use user observable...
  baseUrl = 'http://localhost:5000/api/auth';
  helper = new JwtHelperService();

  private user = new Subject<User>();
  readonly user$ = this.user.asObservable();

  constructor(private http: HttpClient,
              private localStorage: LocalStorageService) {}

  public getToken(): string {
    return this.localStorage.get('token');
  }

  setUser(user: User) {
    this.user.next(user);
  }

  resetUser() {
    this.user.next(null);
  }

  login(user: any) {
    return this.http.post(this.baseUrl + '/login', user);
  }
  register(user: any) {
    return this.http.post(this.baseUrl + '/register', user);
  }

  public isAuthenticated(): boolean {
    // get the token
    const token = this.getToken();

    // return a boolean reflecting
    // whether or not the token is expired
    return !!token && this.helper.isTokenExpired(token);
  }
}
