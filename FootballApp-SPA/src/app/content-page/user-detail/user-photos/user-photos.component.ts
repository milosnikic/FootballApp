import { PhotosService } from "./../../../_services/photos.service";
import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-user-photos",
  templateUrl: "./user-photos.component.html",
  styleUrls: ["./user-photos.component.css"]
})
export class UserPhotosComponent implements OnInit {
  selectedFile: File;

  constructor(private photosService: PhotosService) {}

  ngOnInit() {}

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
  }

  onUpload() {
    console.log(this.selectedFile);
    if (!this.selectedFile) {
      return;
    }
    const formData = new FormData();
    formData.append("file", this.selectedFile);
    this.photosService.uploadPhoto(formData).subscribe(
      res => {
        console.log(res);
      },
      err => {
        console.log(err);
      }
    );
  }
}
