import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap, exhaustMap, catchError, throwError, Observable } from 'rxjs';
import { User } from '../models/user.model';
import { ErrorHandlingService } from '../../../shared/services/error-handling.service';

interface LoginResponseData {
  accessToken: string;
  expiresIn: number;
  firstName: string;
  lastName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user = new BehaviorSubject<User|null>(null);
  
  constructor(private readonly http: HttpClient, private readonly errorHandlingService: ErrorHandlingService) { }

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
            console.log("Login response data: ", accessData);
            user = new User(
              accessData.accessToken,
              accessData.expiresIn,
              accessData.firstName,
              accessData.lastName
            );
            this.user.next(user);

            this.autoLogout(accessData.expiresIn);
        }),
        exhaustMap(accessToken => {
          console.log("Access token: ", accessToken);
          const headers = { 'Authorization': `Bearer ${accessToken.accessToken}` }
          return this.http.get<{id: string, email: string, firstName:string, lastName: string}>('https://localhost:7281/users/me', {headers});
        }),
        tap(userData => {
          console.log("User data: ", userData);
          user.firstName = userData.firstName;
          user.lastName = userData.lastName;
          this.user.next(user);
        }),
        catchError(this.errorHandlingService.handleError) // TODO: find how to handle only the error of the first request (login)
      )
  }

  autoLogout(expiresIn: number) {
    setTimeout(() => {
      this.user.next(null);
    }, expiresIn * 1000);
  }

  logout() {
    this.user.next(null);
  }
}
