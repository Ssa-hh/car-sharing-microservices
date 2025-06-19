import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap, exhaustMap } from 'rxjs';
import { User } from '../models/user.model';

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
  
  constructor(private readonly http: HttpClient) { }

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
      );
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
        })
      )
  }

  autoLogout(expiresIn: number) {
    setTimeout(() => {
      this.user.next(null);
    }, expiresIn * 1000);
  }
}
