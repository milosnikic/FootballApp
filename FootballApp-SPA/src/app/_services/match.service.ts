import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, first } from "rxjs/operators";
import { MatchStatus } from "../_models/matchStatus.enum";

@Injectable({
  providedIn: "root",
})
export class MatchService {
  baseUrl: string = "http://localhost:5000/api/matches";

  constructor(private http: HttpClient) {}

  getUpcomingMatchInfo(matchId: number) {
    return this.http.get(this.baseUrl + `/${matchId}`).pipe(
      first(),
      map((match: any) => {
        return {
          ...match,
          appliedUsers: match.appliedUsers.map((user: any) => ({
            ...user,
            matchStatus: this.mapMatchStatus(
              user.matchStatus.confirmed,
              user.matchStatus.checked
            ),
          })),
        };
      })
    );
  }

  getUpcomingMatchesForGroup(groupId: number) {
    return this.http.get(this.baseUrl + `/upcoming-matches/${groupId}`);
  }

  getUpcomingMatchesForUser(userId: number) {
    return this.http.get(this.baseUrl + `/upcoming-matches?userId=${userId}`);
  }

  getUpcomingMatchesApplicableForUser(userId: number) {
    return this.http.get(this.baseUrl + `/upcoming-matches-applicable?userId=${userId}`);
  }

  getUserMatchStatus(matchId: number, userId: number) {
    return this.http.get(this.baseUrl + `/${matchId}/status/${userId}`);
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

  confirm(userId: number, matchId: number) {
    return this.http.post(
      this.baseUrl + `/${matchId}/confirm?userId=${userId}`,
      {}
    );
  }

  giveUp(userId: number, matchId: number) {
    return this.http.post(
      this.baseUrl + `/${matchId}/give-up?userId=${userId}`,
      {}
    );
  }

  private mapMatchStatus(confirmed: boolean, checked: boolean) {
    return confirmed
      ? MatchStatus.Confirmed
      : checked
      ? MatchStatus.Checked
      : null;
  }
}
