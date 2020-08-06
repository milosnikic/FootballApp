import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { PhotosService } from 'src/app/_services/photos.service';
import { NotifyService } from 'src/app/_services/notify.service';
import { City } from 'src/app/_models/city';
import { Country } from 'src/app/_models/country';
import { LocationsService } from 'src/app/_services/locations.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css'],
})
export class UserSettingsComponent implements OnInit {
  // TODO: Load all countries at start of application
  @Input() userId: number;
  editForm: FormGroup;
  user: User;
  countries: Country[];
  
  country: Country;
  city: City;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private notifyService: NotifyService,
    private locationService: LocationsService
  ) {}

  ngOnInit() {
    this.buildForm();
    this.userService.getUserData(this.userId).subscribe(
      (res: User) => {
        this.user = res;
        this.populateForm(res);
      }
    );
    this.locationService.getAllCountries().subscribe(
      (res: Country[]) => {
        this.countries = res;
      }
    );
  }

  populateForm(user: User) {
    this.country = this.countries.find(
      (country) => country.name === this.user.country
    );
    this.city = this.country.cities.find((city) => city.name === user.city);

    this.editForm = this.fb.group({
      firstname: [user.firstname],
      lastname: [user.lastname],
      username: [user.username],
      email: [user.email],
      country: [this.country.id],
      city: [this.city.id],
    });
  }

  buildForm() {
    this.editForm = this.fb.group({
      firstname: [{ value: '', disabled: true }],
      lastname: [{ value: '', disabled: true }],
      username: [{ value: '', disabled: true }],
      email: [''],
      country: [''],
      city: [''],
    });
  }

  updateUser(userId: number) {
    const data = {
      email: this.editForm.get('email').value,
      country: this.country.id,
      city: this.city.id,
    };
    

    this.userService.updateUser(userId, data).subscribe(
      (res: any) => {
        if (res.key) {
          this.notifyService.showSuccess('Successfully updated profile info!');
        } else {
          this.notifyService.showError(res.value);
        }
      },
      (err) => {
        this.notifyService.showError(err.value);
      }
    );
  }

  setCountry(countryId: number) {
    this.country = this.countries.find(c => c.id === +countryId);
    this.city = this.country.cities[0];
  }
}
