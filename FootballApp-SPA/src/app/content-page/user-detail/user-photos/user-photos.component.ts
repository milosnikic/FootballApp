import { PhotosService } from './../../../_services/photos.service';
import { Component, OnInit, OnDestroy, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import {
  FormControl,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { NotifyService } from 'src/app/_services/notify.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-photos',
  templateUrl: './user-photos.component.html',
  styleUrls: ['./user-photos.component.css'],
})
export class UserPhotosComponent implements OnInit {
  @Input() editable;
  selectedFile: File;
  photos: Photo[];
  uploadForm: FormGroup;
  @Input() userId: number;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private photosService: PhotosService,
    private notifyService: NotifyService,
    private localStorage: LocalStorageService
  ) {}

  ngOnInit() {
    // TODO: Change user id to be passed as input parameter
    this.photosService
      .getPhotosForUser(this.userId)
      .subscribe((res: Photo[]) => {
        this.photos = res;
      });
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
      (photo: any) => {
        if (photo || !photo.key) {
          this.photos.push(photo);
          this.notifyService.showSuccess('Image has been upload succesfully!');
          this.uploadForm.reset();
        } else {
          this.notifyService.showError(photo.value);
        }
      },
      (err) => {
        this.notifyService.showError('Problem uploading image!');
      }
    );
  }

  makePhotoMain(photoId: number) {
    this.photosService.makePhotoMain(photoId, this.userId).subscribe(
      (res: any) => {
        if (res.key) {
          if (!!this.photos.find((p) => p.isMain)) {
            this.photos.find((p) => p.isMain).isMain = false;
          }
          this.photos.find((p) => p.id === photoId).isMain = true;
          window.location.reload();
          this.notifyService.showSuccess('Successfully set profile image.');
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
