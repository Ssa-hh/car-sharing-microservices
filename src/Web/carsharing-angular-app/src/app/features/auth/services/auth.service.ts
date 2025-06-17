import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

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
}
