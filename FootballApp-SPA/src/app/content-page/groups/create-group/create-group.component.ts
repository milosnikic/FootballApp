import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GroupsService } from 'src/app/_services/groups.service';
import { LocalStorageService } from 'src/app/_services/local-storage.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotifyService } from 'src/app/_services/notify.service';
import { Country } from 'src/app/_models/country';
import { LocationsService } from 'src/app/_services/locations.service';
import { City } from 'src/app/_models/city';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css'],
})
export class CreateGroupComponent implements OnInit {
  public imagePath;
  selectedFile: File = null;
  imgURL: any;
  public message: string;
  createGroupForm: FormGroup;
  countries: Country[];
  cities: City[] = [];
  @Output() mainTab = new EventEmitter<number>();

  constructor(
    private groupService: GroupsService,
    private localStorage: LocalStorageService,
    private notifyService: NotifyService,
    private fb: FormBuilder,
    private locationService: LocationsService
  ) {}

  ngOnInit() {
    this.buildForm();
    this.locationService.getAllCountries().subscribe(
      (res: Country[]) => {
        this.countries = res;
        this.getCitiesForCountry(res[0].id);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  buildForm() {
    this.createGroupForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      location: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
    });
  }

  preview(files) {
    if (files.length === 0) return;

    var mimeType = files[0].type;
    this.selectedFile = files[0];
    if (mimeType.match(/image\/*/) == null) {
      this.message = 'Only images are supported.';
      return;
    }
    this.message = null;
    var reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    };
  }

  createGroup() {
    const userId = JSON.parse(this.localStorage.get('user')).id;
    const formData = new FormData();
    
    if(!!this.selectedFile)
      formData.append('image', this.selectedFile, this.selectedFile.name);
    formData.append('name', this.createGroupForm.get('name').value);
    formData.append(
      'description',
      this.createGroupForm.get('description').value
    );
    formData.append('location', this.createGroupForm.get('location').value);
    formData.append('countryId', this.createGroupForm.get('country').value);
    formData.append('cityId', this.createGroupForm.get('city').value);

    this.groupService.createGroup(userId, formData).subscribe((res: any) => {
      if (res.key) {
        this.notifyService.showSuccess(res.value);
        // Switch to first tab
        this.mainTab.emit(0);
      } else {
        this.notifyService.showError(res.value);
      }
    });
  }

  getCitiesForCountry(id: number) {
    this.locationService.getAllCitiesForCountry(id).subscribe(
      (res: City[]) => {
        this.cities = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }
  
  
}
