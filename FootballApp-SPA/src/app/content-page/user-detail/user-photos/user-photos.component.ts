import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-user-photos",
  templateUrl: "./user-photos.component.html",
  styleUrls: ["./user-photos.component.css"]
})
export class UserPhotosComponent implements OnInit {
  selectedFile: File;

  constructor() {}

  ngOnInit() {}

  onFileChanged(event) {
    this.selectedFile = event.target.files[0];
  }

  onUpload() {
    console.log(this.selectedFile);
  }
}
