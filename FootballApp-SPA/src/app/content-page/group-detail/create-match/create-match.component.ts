import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { City } from 'src/app/_models/city';
import { Country } from 'src/app/_models/country';
import { LocationsService } from 'src/app/_services/locations.service';
import { MatchService } from 'src/app/_services/match.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { NotifyService } from 'src/app/_services/notify.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';

@Component({
  selector: 'app-create-match',
  templateUrl: './create-match.component.html',
  styleUrls: ['./create-match.component.css']
})
export class CreateMatchComponent implements OnInit {
  createMatchForm: FormGroup;
  countries: Country[];
  cities: City[] = [];
  minDate = new Date();
  groupId: number;
  @Output()
  upcomingTab = new EventEmitter<number>();

  constructor(private route: ActivatedRoute,
              private fb: FormBuilder,
              private locationService: LocationsService,
              private matchService: MatchService,
              private notifyService: NotifyService,
              private localStorage: LocalStorageService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(
      (res: ParamMap) => {
        this.groupId = +res.get('groupId');
      }
    )
    this.buildForm();
    this.locationService.getAllCountries().subscribe(
      (res: Country[]) => {
        this.countries = res;
        this.getCitiesForCountry(res[0].id);
      }
    );
  }

  buildForm() {
    this.createMatchForm = this.fb.group({
      matchName: ['', Validators.required],
      description: [''],
      numberOfPlayers: [10],
      datePlaying: ['', Validators.required],
      timePlaying: ['', Validators.required],
      location: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
    });
  }

  createMatch() {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    let date = new Date(this.createMatchForm.get('datePlaying').value);
    let time = this.createMatchForm.get('timePlaying').value;
    const hours = time.split(':')[0];
    const minutes = time.split(':')[1];
    
    const datePlaying = new Date(date.getFullYear(), date.getMonth(), date.getDate(), hours, minutes).toLocaleString();

    const data = {
      groupId: this.groupId,
      name: this.createMatchForm.get('matchName').value,
      description: this.createMatchForm.get('description').value,
      numberOfPlayers: +this.createMatchForm.get('numberOfPlayers').value,
      datePlaying,
      location: this.createMatchForm.get('location').value,
      cityId: +this.createMatchForm.get('city').value,
      countryId: +this.createMatchForm.get('country').value,
    };
    
    this.matchService.createMatch(data, userId).subscribe(
      (res: any) => {
        if(res.key) {
          this.notifyService.showSuccess(res.value);
          // Redirect to upcoming matches tab
          this.createMatchForm.reset();
          this.upcomingTab.emit(1);
        } else {
          this.notifyService.showError(res.value);
        }
      },
      err => this.notifyService.showError(err.error)
    );
  }

  getCitiesForCountry(id: number) {
    this.locationService.getAllCitiesForCountry(id).subscribe(
      (res: City[]) => {
        this.cities = res;
      },
      (err) => {
        
      }
    );
  }
}
