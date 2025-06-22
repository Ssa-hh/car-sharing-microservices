import { Injectable } from '@angular/core';
import { Ride } from '../models/ride.model';
import { catchError, Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ErrorHandlingService } from '../../../shared/services/error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class RidesService {

  constructor(private readonly http:HttpClient, private readonly errorHandlingService: ErrorHandlingService) { }

  Get(pickupCity: string, dropOffCity:string, rideDate: string): Observable<Ride[]|Object> {
    const options = {params: new HttpParams().set("pickupCity", pickupCity).set("dropoffCity", dropOffCity).set('ridedate', rideDate)}
    return this.http.get<Ride[]>("https://localhost:7223/rides", options)
    .pipe(catchError(this.errorHandlingService.handleError))
  }
}
