import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class MatchService {
  baseUrl: string = 'http://localhost:5000/api/matches';

  constructor(private http: HttpClient) {}

  createMatch(data: any, userId: number) {
    return this.http.post(this.baseUrl + `/create?userId=${userId}`, data);
  }
}
