import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  baseUrl: string = `${environment.apiUrl}/api/comments`;

  constructor(private http: HttpClient) {}

  getCommentsForUser(userId: number) {
    return this.http.get(this.baseUrl + `?userId=${userId}`);
  }

  postComment(
    data: {
      commenterId: number;
      commentedId: number;
      content: string;
    },
    userId: number
  ) {
    return this.http.post(this.baseUrl + `?userId=${userId}`, data);
  }
}
