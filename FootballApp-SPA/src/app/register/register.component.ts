import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  user :User = new User();

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }


  onSubmit(){
    this.authService.register(this.user).subscribe(
      res => {
        console.log(res)
      },
      err => {
        console.log(err);
      }
    );
  }

}
