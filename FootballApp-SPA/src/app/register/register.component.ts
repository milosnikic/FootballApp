import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  user: User = new User();
  state = 0;
  // 0 if register
  // 1 if login

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }


  register() {
    this.authService.register(this.user).subscribe(
      res => {
        console.log(res);
      },
      err => {
        console.log(err);
      }
    );
    console.log('Pera 1');
  }

  changeState(val) {
    this.state = val;
    console.log(this.state);
  }

  login() {
    this.authService.login(this.user).subscribe(
      res => {
        console.log(res);
      },
      err => {
        console.log(err);
      }
    );
    console.log('Pera 2');
  }
}
