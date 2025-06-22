import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {

  constructor() { }

  public handleError(errorResp: HttpErrorResponse):Observable<Object>
  {
    let errorMessage:string = 'An unknown error occurred';
    
    if (errorResp.error && errorResp.error.detail)
      errorMessage = errorResp.error.detail;  
    
    return throwError(() => errorMessage);
  }
}
