import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  registerForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
    firstname: ['', Validators.required],
    lastname: ['', Validators.required],
    city: ['', Validators.required],
    country: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    dateOfBirth: ['', Validators.required],
    gender: ['', Validators.required]
  });
  loginForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
    password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]]
  });

  constructor(
    private authService: AuthService,
    private fb: FormBuilder) { }

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
