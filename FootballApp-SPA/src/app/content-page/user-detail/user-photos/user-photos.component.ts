import { PhotosService } from './../../../_services/photos.service';
import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { NotifyService } from 'src/app/_services/notify.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-user-photos',
  templateUrl: './user-photos.component.html',
  styleUrls: ['./user-photos.component.css'],
})
export class UserPhotosComponent implements OnInit {
  selectedFile: File;
  photos: Photo[];
  uploadForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private photosService: PhotosService,
    private notifyService: NotifyService,
    private localStorage: LocalStorageService
  ) {}

  ngOnInit() {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    this.photosService.getPhotosForUser(userId).subscribe(
      (res: Photo[]) => {
        this.photos = res;
      }
    );
    this.uploadForm = this.fb.group({
      description: ['', Validators.required],
    });
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
  }

  onUpload() {
    if (!this.selectedFile) {
      return;
    }
    // TODO: Append current date to formData
    //       and if photo is main, and description
    const formData = new FormData();

    formData.append('file', this.selectedFile, this.selectedFile.name);
    formData.append('description', this.uploadForm.get('description').value);

    this.photosService.uploadPhoto(formData).subscribe(
      (photo: Photo) => {
        if (photo) {
          this.photos.push(photo);
          this.notifyService.showSuccess('Image has been upload succesfully!');
        }
      },
      (err) => {
        console.log(err);
        this.notifyService.showError('Problem uploading image!');
      }
    );
  }
}
