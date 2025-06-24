import { Component, DestroyRef, output } from '@angular/core';
import { RidesService } from '../../services/rides.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Ride } from '../../models/ride.model';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap'
import { ToastService } from '../../../../shared/toast/toast.service';

@Component({
  selector: 'app-search-rides-form',
  standalone: false,
  templateUrl: './search-rides-form.component.html',
  styleUrl: './search-rides-form.component.css'
})
export class SearchRidesFormComponent {
  ridesRetrieved = output<Ride[]>();
  currentDate = new Date();
  currentDateStruct: NgbDateStruct = { year: this.currentDate.getFullYear(), month: this.currentDate.getMonth() + 1, day: this.currentDate.getDate() };
  displayValidationErrors = false;
  
  searchForm = new FormGroup({
    pickupCity: new FormControl('', [Validators.required]),
    dropOffCity: new FormControl('', [Validators.required]),
    rideDate: new FormControl(this.currentDateStruct)
  });

  constructor(private readonly rideService: RidesService, private readonly destroyRef: DestroyRef, private readonly toastService: ToastService) {}

  onSubmit() {
    if(this.searchForm.invalid)
    {
      this.displayValidationErrors = true;
      return;
    }
    else
      this.displayValidationErrors = false;

    let rideDate = this.searchForm.value.rideDate?.year + '-' + this.searchForm.value.rideDate?.month + '-' + this.searchForm.value.rideDate?.day;
    const subscription = this.rideService.Get(<string>this.searchForm.value.pickupCity, <string>this.searchForm.value.dropOffCity, rideDate)
      .subscribe({
        next: (rides) => {
          this.ridesRetrieved.emit(<Ride[]>rides);
        },
        error: (error) => {
          this.toastService.showDanger(error); 
        }
    });

    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  get pickupCityIsInvalid() {
    return (
      ((this.searchForm.controls.pickupCity.touched && this.searchForm.controls.pickupCity.dirty) || this.displayValidationErrors)
      && this.searchForm.controls.pickupCity.invalid
    );
  }

  get dropOffCityIsInvalid() {
    return (
      ((this.searchForm.controls.dropOffCity.touched && this.searchForm.controls.dropOffCity.dirty) || this.displayValidationErrors)
      && this.searchForm.controls.dropOffCity.invalid
    );
  }
}
