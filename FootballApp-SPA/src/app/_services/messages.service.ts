import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserToChat } from "../_models/userToChat";

@Injectable({
  providedIn: "root",
})
export class MessagesService {
  private baseUrl: string = "http://localhost:5000/api/chat";
  constructor(private http: HttpClient) {}

  /**
   * Method is used to fetch private chats for user
   * @param userId user that chats are being fetched
   * @returns all users chats
   */
  public getPrivateChats(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/get-private-chats/${userId}`);
  }

  /**
   * @description Method for creating private chats
   * @param userId Id of user who you wanna create chat with
   * @returns result of creation operation
   */
  public createPrivateChat(userId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/create-private-chat/${userId}`, {});
  }

  /**
   * Method is used to fetch available users for chat
   * @returns available users
   */
  public getAvailableUsers(): Observable<UserToChat[]> {
    return this.http.get<UserToChat[]>(`${this.baseUrl}/available-users`);
  }
}
