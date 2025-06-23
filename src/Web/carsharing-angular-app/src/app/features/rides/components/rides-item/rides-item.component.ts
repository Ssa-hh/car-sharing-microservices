import { Component, computed, input, signal } from '@angular/core';
import { Ride } from '../../models/ride.model';
import { DatetimeService } from '../../../../shared/services/datetime.service';

@Component({
  selector: 'app-rides-item',
  standalone: false,
  templateUrl: './rides-item.component.html',
  styleUrl: './rides-item.component.css'
})
export class RidesItemComponent {
  ride = input.required<Ride>();
  duration = computed(() => this.dateTimeService.GetDuration(this.ride().startsAtUtc, this.ride().endsAtUtc));
  pickupTime = computed(() => { let localPickupDate = this.dateTimeService.getLocalDateFromUtc(this.ride().startsAtUtc);
    return localPickupDate.getHours() + "h" + localPickupDate.getMinutes().toString().padStart(2, "0")});
  dropOffTime = computed(() => { let localPickupDate = this.dateTimeService.getLocalDateFromUtc(this.ride().endsAtUtc);
    return localPickupDate.getHours() + "h" + localPickupDate.getMinutes().toString().padStart(2, "0")});
  
  constructor(public readonly dateTimeService: DatetimeService) {}
}
