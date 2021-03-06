import { Component, OnInit } from "@angular/core";
import { AuthService } from "../_services/auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NotifyService } from "../_services/notify.service";
import { Router } from "@angular/router";
import { LocalStorageService } from "../_services/local-storage.service";
import { City } from "../_models/city";
import { Country } from "../_models/country";
import { LocationsService } from "../_services/locations.service";
import { LandingPageState } from "../constants";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent implements OnInit {
  cities: City[];
  countries: Country[];
  state = LandingPageState.Login;
  LandingPageState = LandingPageState;
  registerForm: FormGroup;
  loginForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private notifyService: NotifyService,
    private router: Router,
    private locationService: LocationsService,
    private localStorage: LocalStorageService
  ) {}

  ngOnInit() {
    this.locationService.getAllCountries().subscribe((res: Country[]) => {
      this.countries = res;
      this.getCitiesForCountry(res[0].id);
    });
    this.buildForms();
  }

  private buildForms(): void {
    this.initializeRegisterForm();
    this.initializeLoginForm();
  }

  private initializeLoginForm(): void {
    this.loginForm = this.fb.group({
      username: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(20),
        ],
      ],
      password: [
        "",
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(20),
        ],
      ],
    });
  }

  private initializeRegisterForm(): void {
    this.registerForm = this.fb.group({
      username: [
        "",
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(20),
        ],
      ],
      password: [
        "",
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(20),
        ],
      ],
      firstname: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(20),
        ],
      ],
      lastname: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(20),
        ],
      ],
      city: ["", Validators.required],
      country: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      dateOfBirth: ["", Validators.required],
      gender: ["", Validators.required],
    });
  }

  public register(): void {
    const registerData = {
      username: this.registerForm.get("username").value,
      password: this.registerForm.get("password").value,
      firstname: this.registerForm.get("firstname").value,
      lastname: this.registerForm.get("lastname").value,
      city: this.registerForm.get("city").value,
      country: this.registerForm.get("country").value,
      email: this.registerForm.get("email").value,
      dateOfBirth: this.registerForm.get("dateOfBirth").value,
      gender: this.registerForm.get("gender").value,
    };
    this.authService.register(registerData).subscribe(
      () => {
        this.notifyService.showSuccess("Successfully registered!");
        this.state = 1;
      },
      (err) => {
        this.notifyService.showError(err);
      }
    );
  }

  public changeState(value: LandingPageState): void {
    this.state = value;
  }

  public login(): void {
    const loginData = {
      username: this.loginForm.get("username").value,
      password: this.loginForm.get("password").value,
    };
    this.authService.login(loginData).subscribe(
      (res: any) => {
        this.notifyService.showSuccess("Successfully logged in!");
        this.localStorage.set("token", res.token);
        this.localStorage.set("user", JSON.stringify(res.user));
        this.router.navigate(["/app/dashboard"]);
      },
      (err) => {
        this.notifyService.showError();
      }
    );
  }

  public getCitiesForCountry(id: number): void {
    this.locationService.getAllCitiesForCountry(id).subscribe((res: City[]) => {
      this.cities = res;
    });
  }
}
