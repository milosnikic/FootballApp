import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class LocationsService {
  private baseUrl: string = 'http://localhost:5000/api/locations';

  constructor(private http: HttpClient) {}

  getAllCountries() {
    return this.http.get(this.baseUrl + '/all-countries');
  }

  getAllCitiesForCountry(id: number) {
    return this.http.get(this.baseUrl + `/country-cities/${id}`);
  }
}
