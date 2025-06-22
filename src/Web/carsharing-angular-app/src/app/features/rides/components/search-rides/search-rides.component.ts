import { Component, signal } from '@angular/core';
import { Ride } from '../../models/ride.model';

@Component({
  selector: 'app-search-rides',
  standalone: false,
  templateUrl: './search-rides.component.html',
  styleUrl: './search-rides.component.css'
})
export class SearchRidesComponent {
  rides = signal<Ride[]>([]);

  getSearchedRides(rides: Ride[]) {
    this.rides.set(rides);
  }
}
