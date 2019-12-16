import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

baseUrl : string = "http://localhost:5000/api/auth";

constructor(private http: HttpClient) { }

login(user: any){

}
register(user:any){
  return this.http.post(this.baseUrl + '/register', user);
}
}
