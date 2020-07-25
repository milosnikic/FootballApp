import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth';
  helper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  public getToken(): string {
    return localStorage.getItem('token');
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
