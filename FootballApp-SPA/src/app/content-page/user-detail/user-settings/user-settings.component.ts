import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { PhotosService } from 'src/app/_services/photos.service';
import { NotifyService } from 'src/app/_services/notify.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css']
})
export class UserSettingsComponent implements OnInit {

  @Input() userId: number;
  editForm: FormGroup;
  user: User;

  constructor(private fb: FormBuilder,
              private userService: UserService,
              private notifyService: NotifyService) { }

  ngOnInit() {
    this.buildForm();
    this.userService.getUserData(this.userId).subscribe(
      (res: User) => {
        this.user = res;
        this.populateForm(res);
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  populateForm(user: User) {
    this.editForm = this.fb.group({
      firstname: [user.firstname],
      lastname: [user.lastname],
      username: [user.username],
      email: [user.email],
      country: [user.country],
      city: [user.city],
    });
  }

  buildForm() {
    this.editForm = this.fb.group({
      firstname: [{value: '', disabled: true}],
      lastname: [{value: '', disabled: true}],
      username: [{value: '', disabled: true}],
      email: [''],
      country: [''],
      city: [''],
    });
  }

  updateUser(userId: number) {
    const data = {
      email: this.editForm.get('email').value,
      city: this.editForm.get('city').value,
      country: this.editForm.get('country').value
    };

    this.userService.updateUser(userId, data).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess('Successfully updated profile info!');
        }else {
          this.notifyService.showError(res.value);
        }
      },
      err => {
        this.notifyService.showError(err);
      }
    );
  }
}
