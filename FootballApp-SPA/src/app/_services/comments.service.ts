import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  baseUrl: string = 'http://localhost:5000/api/comments';

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
