import { RegisterComponent } from './register.component';
import { LocationsService } from '../_services/locations.service';
import { of, Observable } from 'rxjs';
import { FormBuilder } from '@angular/forms';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let locationService: LocationsService;

  beforeEach(() => {
    locationService = new LocationsService(null);
    component = new RegisterComponent(
      null,
      new FormBuilder(),
      null,
      null,
      locationService,
      null
    );
  });

  it('should load cites for country from backend server', () => {
    spyOn(locationService, 'getAllCitiesForCountry').and.callFake((id) => {
      return of(['Belgrade', 'Novi Sad', 'Nis']);
    });

    component.getCitiesForCountry(null);

    expect(component.cities.length).toBeGreaterThan(0);
    expect(component.cities.length).toBe(3);
  });

  it('should create login form with two controls', () => {
    expect(component.loginForm.contains('username')).toBe(true);
    expect(component.loginForm.contains('password')).toBe(true);
  });

  it('should check for login form empty values', () => {
    // Arrange
    const usernameControl = component.loginForm.get('username');
    const passwordControl = component.loginForm.get('password');
    // Act
    usernameControl.setValue('');
    passwordControl.setValue('');
    // Assert
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('should check for values less than 5', () => {
    // Arrange
    const usernameControl = component.loginForm.get('username');
    const passwordControl = component.loginForm.get('password');
    // Act
    usernameControl.setValue('asd');
    passwordControl.setValue('asd');
    //Assert
    expect(component.loginForm.valid).toBeFalsy();
  });
});
