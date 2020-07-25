import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  baseUrl: string = 'http://localhost:5000/api/photos';

  constructor(private http: HttpClient,
              private localStorage: LocalStorageService) { }

  uploadPhoto(formData: FormData) {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    return this.http.post(this.baseUrl + `?userId=${userId}`, formData);
  }

  getPhotosForUser(userId: number){
    return this.http.get(this.baseUrl + `?userId=${userId}`);
  }
}
