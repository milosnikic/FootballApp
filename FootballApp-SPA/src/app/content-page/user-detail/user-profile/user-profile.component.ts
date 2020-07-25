import { User } from './../../../_models/user';
import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  @Input() user: User;
  @Input() editable: boolean;
  @Output() editProfile = new EventEmitter<number>();
  constructor() { }

  ngOnInit() {
  }

  onEditProfile(){
    this.editProfile.emit(5);
  }

}