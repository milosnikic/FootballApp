import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotifyService } from '../_services/notify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  state = 0;
  // 0 if register
  // 1 if login
  registerForm: FormGroup;
  loginForm: FormGroup;
  

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private notifyService: NotifyService,
    private router: Router) { }

  ngOnInit() {
    this.buildForms();
  }

  buildForms() {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      firstname: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      lastname: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      city: ['', Validators.required],
      country: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required]
    });
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]]
    });
  }


  register() {
    const registerData = {
      username: this.registerForm.get('username').value,
      password: this.registerForm.get('password').value,
      firstname: this.registerForm.get('firstname').value,
      lastname: this.registerForm.get('lastname').value,
      city: this.registerForm.get('city').value,
      country: this.registerForm.get('country').value,
      email: this.registerForm.get('email').value,
      dateOfBirth: this.registerForm.get('dateOfBirth').value,
      gender: this.registerForm.get('gender').value,
    };
    this.authService.register(registerData).subscribe(
      res => {
        this.notifyService.showSuccess('Successfully registered!');
        this.state = 1;
      },
      err => {
        this.notifyService.showError(err);
      }
    );
  }

  changeState(val) {
    this.state = val;
  }

  login() {
    const loginData = {
      username: this.loginForm.get('username').value,
      password: this.loginForm.get('password').value
    };
    this.authService.login(loginData).subscribe(
      (res: any) => {
        this.notifyService.showSuccess('Successfully logged in!');
        this.router.navigate(['/app/dashboard']);
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res.user));
      },
      err => {
        this.notifyService.showError();
      }
    );
  }
}
