import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  baseUrl: string = 'http://localhost:5000/api/photos';

  constructor(private http: HttpClient) { }

  uploadPhoto(formData: FormData) {
    const headers = new HttpHeaders({
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    });
    const userId = JSON.parse(localStorage.getItem('user')).id;
    return this.http.post(this.baseUrl + `?userId=${userId}`, formData, {headers});
  }
}
