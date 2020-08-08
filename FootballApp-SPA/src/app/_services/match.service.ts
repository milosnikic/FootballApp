import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class MatchService {
  baseUrl: string = 'http://localhost:5000/api/matches';

  constructor(private http: HttpClient) {}

  getUpcomingMatchInfo(matchId: number) {
    return this.http.get(this.baseUrl + `/${matchId}`);
  }

  getUpcomingMatches(groupId: number) {
    return this.http.get(this.baseUrl + `/upcoming-matches?groupId=${groupId}`);
  }
  createMatch(data: any, userId: number) {
    return this.http.post(this.baseUrl + `/create?userId=${userId}`, data);
  }

  checkIn(userId: number, matchId: number) {
    return this.http.post(
      this.baseUrl + `/${matchId}/check-in?userId=${userId}`,
      {}
    );
  }
}
