import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  baseUrl: string = `${environment.apiUrl}/api/photos`;

  constructor(private http: HttpClient,
              private localStorage: LocalStorageService) { }

  uploadPhoto(formData: FormData) {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    return this.http.post(this.baseUrl + `?userId=${userId}`, formData);
  }

  getPhotosForUser(userId: number){
    return this.http.get(this.baseUrl + `?userId=${userId}`);
  }

  makePhotoMain(photoId: number, userId: number) {
    return this.http.post(this.baseUrl + '/main' + `?photoId=${photoId}&userId=${userId}`, {});
  }
}
