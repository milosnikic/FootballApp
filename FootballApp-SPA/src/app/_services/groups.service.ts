import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subject } from "rxjs";
import { Group } from "../_models/group";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: "root",
})
export class GroupsService {
  baseUrl: string = `${environment.apiUrl}/api/groups"`
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

  getAllGroups(userId: number) {
    return this.http
      .get(this.baseUrl + "/all" + `?userid=${userId}`)
      .subscribe((res: Group[]) => {
        this.allGroups.next(res);
      });
  }

  getUsersGroups(userId: number) {
    return this.http
      .get(this.baseUrl + `?userId=${userId}`)
      .subscribe((res: any) => {
        this.usersGroups.next(res);
      });
  }

  getUsersCreatedGroups(userId: number) {
    return this.http
      .get(this.baseUrl + "/created" + `?userId=${userId}`)
      .subscribe((res: any) => {
        this.usersCreatedGroups.next(res);
      });
  }

  getUsersFavoriteGroups(userId: number) {
    return this.http
      .get(this.baseUrl + `/favorite?userId=${userId}`)
      .subscribe((res: any) => {
        this.usersFavoriteGroups.next(res);
      });
  }

  getMembershipInformation(groupId: number, userId: number) {
    return this.http.get(this.baseUrl + `/${groupId}/membership-info?userId=${userId}`);
  }

  getDetailGroupInformation(groupId: number, userId: number) {
    return this.http.get(this.baseUrl + `/${groupId}?userId=${userId}`);
  }

  requestToJoin(userId: number, groupId: number) {
    return this.http.post(
      this.baseUrl + `/request-join/${groupId}` + `?userId=${userId}`,
      {}
    );
  }

  makeFavorite(userId: number, groupId: number) {
    return this.http.post(
      this.baseUrl + `/favorite/${groupId}` + `?userId=${userId}`,
      {}
    );
  }

  makeUnfavorite(userId: number, groupId: number, favorite: boolean) {
    return this.http.post(
      this.baseUrl +
        `/favorite/${groupId}` +
        `?userId=${userId}&favorite=${favorite}`,
      {}
    );
  }

  leaveGroup(userId: number, groupId: number) {
    return this.http.post(
      this.baseUrl + `/leave/${groupId}` + `?userId=${userId}`,
      {}
    );
  }

  acceptUser(userId: number, groupId: number) {
    return this.http.post(
      this.baseUrl + `/accept/${groupId}?userId=${userId}`,
      {}
    );
  }

  rejectUser(userId: number, groupId: number) {
    return this.http.delete(
      this.baseUrl + `/reject/${groupId}?userId=${userId}`,
      {}
    );
  }
}
