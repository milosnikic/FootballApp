import { RegisterComponent } from './register.component';
import { LocationsService } from '../_services/locations.service';
import { of, Observable } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let locationService: LocationsService;
  let authService: AuthService;

  beforeEach(() => {
    locationService = new LocationsService(null);
    authService = new AuthService(null, null);
    component = new RegisterComponent(
      authService,
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

  it('should check for values greater than 20', () => {
    // Arrange
    const usernameControl = component.loginForm.get('username');
    const passwordControl = component.loginForm.get('password');
    // Act
    usernameControl.setValue('asd12asd12asd12asd12asd12asd12asd12');
    passwordControl.setValue('asd12asd12asd12asd12asd12asd12asd12');
    //Assert
    expect(component.loginForm.valid).toBeFalsy();
  });

  it('should get all countries from backend', () => {
    spyOn(locationService, 'getAllCountries').and.callFake(() => {
      return of(['Serbia', 'Bulgaria', 'Croatia', 'Bosnia and Herzegovina']);
    });

    component.ngOnInit();

    expect(component.countries.length).toBe(4);
  });

  it('should create register form', () => {
    expect(component.registerForm.contains('username')).toBeTruthy();
    expect(component.registerForm.contains('password')).toBeTruthy();
    expect(component.registerForm.contains('firstname')).toBeTruthy();
    expect(component.registerForm.contains('lastname')).toBeTruthy();
    expect(component.registerForm.contains('city')).toBeTruthy();
    expect(component.registerForm.contains('country')).toBeTruthy();
    expect(component.registerForm.contains('email')).toBeTruthy();
    expect(component.registerForm.contains('dateOfBirth')).toBeTruthy();
    expect(component.registerForm.contains('gender')).toBeTruthy();
  });

  it('should check for register form empty values', () => {
    const usernameControl = component.registerForm.get('username');
    const passwordControl = component.registerForm.get('password');
    const firstnameControl = component.registerForm.get('firstname');
    const lastnameControl = component.registerForm.get('lastname');
    const cityControl = component.registerForm.get('city');
    const countryControl = component.registerForm.get('country');
    const emailControl = component.registerForm.get('email');
    const dateOfBirthControl = component.registerForm.get('dateOfBirth');
    const genderControl = component.registerForm.get('gender');
    usernameControl.setValue('');
    passwordControl.setValue('');
    firstnameControl.setValue('');
    lastnameControl.setValue('');
    cityControl.setValue('');
    countryControl.setValue('');
    emailControl.setValue('');
    dateOfBirthControl.setValue('');
    genderControl.setValue('');

    expect(component.registerForm.valid).toBeFalsy();
  });

  // Login method test
  it('should call login method with parameters', () => {
    const usernameControl = component.loginForm.get('username');
    const passwordControl = component.loginForm.get('password');
    
    usernameControl.setValue('milos');
    passwordControl.setValue('nikic');

    const data = component.loginForm.value;

    let spy = spyOn(authService, 'login').and.callFake((data) => {
      return of();
    });

    component.login();

    expect(spy).toHaveBeenCalledWith(data);
  });

  // Register method test
  it('should call register method with parameters', () => {
    const usernameControl = component.registerForm.get('username');
    const passwordControl = component.registerForm.get('password');
    const firstnameControl = component.registerForm.get('firstname');
    const lastnameControl = component.registerForm.get('lastname');
    const cityControl = component.registerForm.get('city');
    const countryControl = component.registerForm.get('country');
    const emailControl = component.registerForm.get('email');
    const dateOfBirthControl = component.registerForm.get('dateOfBirth');
    const genderControl = component.registerForm.get('gender');

    usernameControl.setValue('milos');
    passwordControl.setValue('nikic');
    firstnameControl.setValue('test');
    lastnameControl.setValue('test');
    cityControl.setValue('test');
    countryControl.setValue('test');
    emailControl.setValue('test');
    dateOfBirthControl.setValue('test');
    genderControl.setValue('test');

    const data = component.registerForm.value;

    let spy = spyOn(authService, 'register').and.callFake((data) => {
      return of();
    });
    
    component.register();

    expect(spy).toHaveBeenCalledWith(data);
  });
});
