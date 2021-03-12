import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { User } from "../_models/user";

@Injectable({
  providedIn: "root",
})
export class FriendsService {
  private baseUrl: string = "http://localhost:5000/api/friends";
  private readonly exploreUsersSource = new Subject<User[]>();
  private readonly friendsSource = new Subject<User[]>();
  private readonly pendingRequestsSource = new Subject<User[]>();
  private readonly sentFriendRequestsSource = new Subject<User[]>();

  exploreUsers$ = this.exploreUsersSource.asObservable();
  friends$ = this.friendsSource.asObservable();
  pendingRequests$ = this.pendingRequestsSource.asObservable();
  sentFriendRequests$ = this.sentFriendRequestsSource.asObservable();

  constructor(private http: HttpClient) {}

  public getAllFriendsForUser(userId: number): void {
    this.http.get(`${this.baseUrl}/${userId}`).subscribe((res: User[]) => {
      this.friendsSource.next(res);
    });
  }

  public pendingFriendRequests(userId: number): void {
    this.http
      .get(`${this.baseUrl}/pending-requests/${userId}`)
      .subscribe((res: User[]) => {
        this.pendingRequestsSource.next(res);
      });
  }

  public sentFriendRequests(userId: number): void {
    this.http
      .get(`${this.baseUrl}/sent-requests/${userId}`)
      .subscribe((res: User[]) => {
        this.sentFriendRequestsSource.next(res);
      });
  }

  public getAllExploreUsers(userId: number): void {
    this.http
      .get(`${this.baseUrl}/explore/${userId}`)
      .subscribe((res: User[]) => {
        this.exploreUsersSource.next(res);
      });
  }

  public sendFriendRequest(data: {
    senderId: number;
    receiverId: number;
  }): Observable<any> {
    return this.http.post(`${this.baseUrl}/send-request`, data);
  }

  public acceptFriendRequest(data: {
    senderId: number;
    receiverId: number;
  }): Observable<any> {
    return this.http.post(`${this.baseUrl}/accept-request`, data);
  }

  public deleteFriendRequest(data: {
    senderId: number;
    receiverId: number;
  }): Observable<any> {
    const options = {
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      }),
      body: data,
    };
    return this.http.delete(`${this.baseUrl}/delete-request`, options);
  }
}
