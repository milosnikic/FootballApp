import { User } from './../../../_models/user';
import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Visitor } from 'src/app/_models/visitor';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  @Input() user: User;
  @Input() editable: boolean;
  @Output() editProfile = new EventEmitter<number>();
  visitors: Visitor[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getLatestFiveVisitorsForUser(this.user.id)
      .subscribe(
        (res: Visitor[]) =>{
          this.visitors = res;
        },
        err => {
          console.log(err);
          console.log('error fetching visitors');
        }
      );
  }

  onEditProfile(){
    this.editProfile.emit(5);
  }

}
