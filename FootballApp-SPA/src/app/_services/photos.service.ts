import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  baseUrl = 'http://localhost:5000/api/users/1/photos';

  constructor(private http: HttpClient) { }

  uploadPhoto(){
    // return this.http.post(this.baseUrl)
    // todo
  }
}
