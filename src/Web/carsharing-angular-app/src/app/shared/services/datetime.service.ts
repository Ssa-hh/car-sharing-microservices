import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DatetimeService {

  constructor() { }

  // TODO: change the parameter type to date
  getLocalDateFromUtc(dateUtc: string): Date
  {
    let dateUtcAsDate = new Date(dateUtc);
    let offset = dateUtcAsDate.getTimezoneOffset();

    return new Date(dateUtcAsDate.getTime() - offset * 60000);
  }

  getUtcDateFromLocal(dateUtc: Date): Date
  {
    let offset = dateUtc.getTimezoneOffset();

    return new Date(dateUtc.getTime() + offset * 60000);
  }

  GetDuration(startDate: string, endDate:string):string {
    
    let startDateAsDate = new Date(startDate);
    let endDateAsDate = new Date(endDate);

    let diffInMinutes = Math.round((endDateAsDate.getTime() - startDateAsDate.getTime())/(1000*60));

    let minutes = diffInMinutes % 60;

    let hours = Math.round((diffInMinutes - minutes)/60);

    return hours + "h" + minutes.toString().padStart(2, "0");
  }
}
