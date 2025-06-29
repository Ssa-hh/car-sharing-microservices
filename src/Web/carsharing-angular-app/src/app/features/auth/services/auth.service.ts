import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap, exhaustMap, catchError, throwError, Observable } from 'rxjs';
import { User } from '../models/user.model';
import { Car } from '../../../shared/models/car.model';
import { ErrorHandlingService } from '../../../shared/services/error-handling.service';
import { Router } from '@angular/router';

interface LoginResponseData {
  accessToken: string;
  expireIn: number;
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user = new BehaviorSubject<User|null>(null);
  private tokenExpirationTimer: any
  
  constructor(private http: HttpClient, private errorHandlingService: ErrorHandlingService, private router: Router) { }

  register(email: string, password: string, firstName: string, lastName: string) {
    return this.http
      .post(
        'https://localhost:7281/users/register',
        {
          email: email,
          password: password,
          firstName: firstName,
          lastName: lastName
        }
      )
      .pipe(catchError(this.errorHandlingService.handleError));
  }

  login(email: string, password: string) {
    let user: User;
    
    return this.http
      .post<LoginResponseData>(
        'https://localhost:7281/users/login',
        {
          email: email,
          password: password
        }
      ).pipe(
        tap(accessData => 
          {
            let tokenExpirationDate = new Date();
            tokenExpirationDate.setSeconds(tokenExpirationDate.getSeconds() + accessData.expireIn);
            user = new User(
              accessData.accessToken,
              tokenExpirationDate,
              accessData.firstName,
              accessData.lastName,
              []
            );
            this.user.next(user);

            this.autoLogout(accessData.expireIn);
        }),
        exhaustMap(accessToken => {
          const headers = { 'Authorization': `Bearer ${accessToken.accessToken}` }
          return this.http.get<{id: string, email: string, firstName:string, lastName: string, cars: Car[]}>('https://localhost:7281/users/me', {headers});
        }),
        tap(userData => {
          user.firstName = userData.firstName;
          user.lastName = userData.lastName;
          user.cars = userData.cars;
          this.user.next(user);
          sessionStorage.setItem('userData', JSON.stringify(user));
        }),
        catchError(this.errorHandlingService.handleError) // TODO: find how to handle only the error of the first request (login)
      )
  }

  autoLogin() {
    const userDataString = sessionStorage.getItem('userData');
    
    if(!userDataString) {
      return;
    }
    
    const userData: {_accessToken: string, _tokenExpirationDate: string, firstName: string, 
              lastName: string, cars: Car[]} = JSON.parse(userDataString);
    if(!userData) {
      return;
    }

    const loadedUser = new User(userData._accessToken, new Date(userData._tokenExpirationDate), userData.firstName, userData.lastName, userData.cars);

    if(loadedUser.accessToken) {
      this.user.next(loadedUser);
      let expirationDuration = (new Date(userData._tokenExpirationDate).getTime() - new Date().getTime()) / 1000; // in seconds
      this.autoLogout(expirationDuration);
    }
  }

  autoLogout(expiresIn: number) {
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expiresIn * 1000);
  }

  logout() {
    this.user.next(null);
    this.router.navigate(['/login']);
    
    if(this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
    this.tokenExpirationTimer = null;
  }

  addCar(brand: string, model: string, numberOfSeats: number, colorHexCode?: string|null) {
    const headers = { 'Authorization': `Bearer ${this.user.value?.accessToken}` }

    return this.http
      .post<string>(
        'https://localhost:7281/users/me/cars',
        {
          brand: brand,
          model: model,
          numberOfSeats: numberOfSeats,
          colorHexCode: colorHexCode
        },
        { headers }
      )
      .pipe(
        tap(carId => {
          const newCar:Car = {id: carId, brand: brand, model: model, numberOfSeats: numberOfSeats, colorHexCode: colorHexCode};
          this.user.value?.cars?? [];
          this.user.value?.cars.push(newCar);
          this.user.next(this.user.value);
        }),
        catchError(this.errorHandlingService.handleError)
      );
  }
}
