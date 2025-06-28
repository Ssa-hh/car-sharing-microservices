import { Injectable } from '@angular/core';
import { Ride } from '../models/ride.model';
import { catchError, exhaustMap, Observable, take, tap } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ErrorHandlingService } from '../../../shared/services/error-handling.service';
import { Car } from '../../../shared/models/car.model';
import { EditRide } from '../models/edit-ride.model';
import { DatetimeService } from '../../../shared/services/datetime.service';
import { AuthService } from '../../auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RidesService {

  constructor(private readonly http:HttpClient, private readonly errorHandlingService: ErrorHandlingService, 
            private readonly dateTimeService: DatetimeService, private readonly authService: AuthService) {}

  Get(pickupCity: string, dropOffCity:string, rideDate: string): Observable<Ride[]|Object> {
    const options = {params: new HttpParams().set("pickupCity", pickupCity).set("dropoffCity", dropOffCity).set('ridedate', rideDate)}
    return this.http.get<Ride[]>("https://localhost:7223/rides", options)
    .pipe(catchError(this.errorHandlingService.handleError))
  }

  post(pickupCity: string, dropOffCity: string, startsAt: Date, duration: {hour:number, minute: number}, numberOfProposedSeats: number, car: Car) {
    let startsAtUtc = this.dateTimeService.getUtcDateFromLocal(startsAt);
    let endsAtUtc = new Date(startsAtUtc.getFullYear(), startsAtUtc.getMonth(), startsAtUtc.getDate(),
                          startsAtUtc.getHours() + duration.hour, startsAtUtc.getMinutes() + duration.minute)
    
    let newRide : EditRide = {id: "", pickupCity: pickupCity, dropOffCity: dropOffCity, startsAtUtc: startsAtUtc, endsAtUtc: endsAtUtc, 
                            numberOfProposedSeats: numberOfProposedSeats, car: car};
    
    return this.authService.user.pipe(
      take(1),
      exhaustMap(user => {
        let headers = { 'Authorization': `Bearer ${user?.accessToken}` };
        return this.http
        .post(
          'https://localhost:7223/rides', newRide, {headers}
        )
        }),
        catchError(this.errorHandlingService.handleError)
    )
  }
}
