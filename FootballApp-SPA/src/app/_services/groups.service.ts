import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GroupsService {

baseUrl: string = 'http://localhost:5000/api/groups';

constructor(private http: HttpClient) { }

createGroup(userId: number, data: any) {
  return this.http.post(this.baseUrl + `?userId=${userId}`, data);
}

}
