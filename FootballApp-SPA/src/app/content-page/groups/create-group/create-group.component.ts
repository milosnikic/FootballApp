import { Component, OnInit } from "@angular/core";
import { GroupsService } from 'src/app/_services/groups.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: "app-create-group",
  templateUrl: "./create-group.component.html",
  styleUrls: ["./create-group.component.css"]
})
export class CreateGroupComponent implements OnInit {
  public imagePath;
  selectedFile: File;
  imgURL: any;
  public message: string;
  createGroupForm: FormGroup;

  constructor(private groupService: GroupsService,
              private localStorage: LocalStorageService,
              private notifyService: NotifyService,
              private fb: FormBuilder) {}

  ngOnInit() {
    this.buildForm();
  }

  buildForm() {
    this.createGroupForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      location: ['', Validators.required]
    });
  }

  preview(files) {
    if (files.length === 0) return;

    var mimeType = files[0].type;
    this.selectedFile = files[0];
    if (mimeType.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }
    this.message = null;
    var reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = _event => {
      this.imgURL = reader.result;
    };
  }

  createGroup() {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    const formData = new FormData();

    formData.append('image', this.selectedFile, this.selectedFile.name);
    formData.append('name', this.createGroupForm.get('name').value);
    formData.append('description', this.createGroupForm.get('description').value);
    formData.append('location', this.createGroupForm.get('location').value);

    this.groupService.createGroup(userId, formData).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
        }else {
          this.notifyService.showError(res.value);
        }
      }
    );
  }
}
