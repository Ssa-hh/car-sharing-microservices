import { Component, signal, OnInit, DestroyRef, computed, WritableSignal, Signal } from '@angular/core';
import { AuthService } from '../../../auth/services/auth.service';
import { Car } from '../../../../shared/models/car.model';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CustomValidators } from '../../../../shared/custom-validators';
import { NgbDateStruct, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { toSignal } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { RidesService } from '../../services/rides.service';
import { ToastService } from '../../../../shared/toast/toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-ride',
  standalone: false,
  templateUrl: './add-ride.component.html',
  styleUrl: './add-ride.component.css'
})
export class AddRideComponent implements OnInit {
  private isAuthenticated = signal<boolean>(false);
  private currentDate = new Date(new Date().setHours(0,0,0,0));
  isLoading = false;
  userCars = signal<Car[]>([]);
  currentTimeStruct: NgbTimeStruct = { hour: this.currentDate.getHours(), minute: this.currentDate.getMinutes(), second: this.currentDate.getSeconds() };
  currentDateStruct: NgbDateStruct = { year: this.currentDate.getFullYear(), month: this.currentDate.getMonth() + 1, day: this.currentDate.getDate() };
  currentStep = signal<number>(Steps.askAuthentication);
  steps = Steps;
  private displayErrors = false;

  // Form controls
  private numberOfProposedSeatsFC = new FormControl(1, [Validators.required, Validators.min(1)]);
  private numberOfCarSeatsFC = new FormControl(2, [Validators.required, Validators.min(2)]);
  private departureDateFC = new FormControl<NgbDateStruct|null>(null, [Validators.required, CustomValidators.minDate(this.currentDate)]);
  private departureTimeFC = new FormControl<NgbTimeStruct>(this.currentTimeStruct, [Validators.required]);

  // Dynamic validators
  private nbrOfProposedSeatsMaxValidator:ValidatorFn = Validators.max(1);
  private departureDateTimeValidator:ValidatorFn = this.departureDateTimeInFuture(this.currentDateStruct);
  
  numberOfCarSeats: Signal<number|null|undefined> = signal(2);

  addRideForm = new FormGroup(
    {
      pickupCity: new FormControl('', [Validators.required]),
      dropOffCity: new FormControl('', [Validators.required]),
      departureDate: this.departureDateFC,
      departureTime: this.departureTimeFC,
      rideDuration: new FormControl<NgbTimeStruct>({ hour: 0, minute: 0, second: 0 }, [Validators.required, CustomValidators.minTime({hour:0, minute:1})]),
      car: new FormGroup({
        brand: new FormControl('', Validators.required),
        model: new FormControl('', Validators.required),
        numberOfSeats: this.numberOfCarSeatsFC,
        colorHexCode: new FormControl('')
      }),
      numberOfProposedSeats: this.numberOfProposedSeatsFC
    }
  );

  constructor(private readonly authService:AuthService, private readonly rideService:RidesService, private readonly destroyRef: DestroyRef, 
              private readonly toastService:ToastService, private readonly router:Router){
    this.numberOfCarSeats = toSignal(this.numberOfCarSeatsFC.valueChanges);
  }

  ngOnInit(): void {
    const userSubscription = this.authService.user.subscribe(user => 
      {
        this.isAuthenticated.set(!!user);
        this.userCars.set(user?.cars??[])
        if(this.isAuthenticated() && this.currentStep() == Steps.askAuthentication)
          this.currentStep.set(Steps.pickupCity);
        else if(!this.isAuthenticated)
          this.currentStep.set(Steps.askAuthentication);
      });
    
    // update dynamic validators
    const numberOfSeatsSubscription = this.numberOfCarSeatsFC.valueChanges.pipe(
      debounceTime(300),  // Wait 300ms after the last keystroke
      distinctUntilChanged() // Ignore if the value hasn't changed
    ).subscribe( value => {
      this.numberOfProposedSeatsFC.removeValidators(this.nbrOfProposedSeatsMaxValidator);
      this.nbrOfProposedSeatsMaxValidator = Validators.max( !!value ? value :  1);
      this.numberOfProposedSeatsFC.addValidators(this.nbrOfProposedSeatsMaxValidator);
    });

    const departureDateSubscription = this.departureDateFC.valueChanges.pipe(
      debounceTime(300),  // Wait 300ms after the last keystroke
      distinctUntilChanged() // Ignore if the value hasn't changed
    ).subscribe( value => {
      this.departureTimeFC.removeValidators(this.departureDateTimeValidator);
      this.departureDateTimeValidator = this.departureDateTimeInFuture(!!value? value : this.currentDateStruct);
      this.departureTimeFC.addValidators(this.departureDateTimeValidator);
    });

    this.destroyRef.onDestroy(() => 
      {
        userSubscription.unsubscribe();
        numberOfSeatsSubscription.unsubscribe();
        departureDateSubscription.unsubscribe();
      });
  }

  isCurrentStepValid():boolean {
    let result = true;
    switch(this.currentStep())
    {
      case Steps.pickupCity:
        result = this.addRideForm.controls.pickupCity.valid;
        break;
      case Steps.dropOffCity:
        result = this.addRideForm.controls.dropOffCity.valid;
        break; 
      case Steps.departureDate:
        result = this.addRideForm.controls.departureDate.valid;
        break;
      case Steps.departureTime:
        result = this.addRideForm.controls.departureTime.valid;
        break;
      case Steps.rideDuration:
        result = this.addRideForm.controls.rideDuration.valid;
        break;
      case Steps.car:
        result = this.addRideForm.controls.car.valid;
        break;
      case Steps.numberOfProposedSeats:
        result = this.addRideForm.controls.numberOfProposedSeats.valid;
        break;
    }
    return result;
  }

  publishRide() {
    if(this.addRideForm.invalid)
      return;

    let startsAt = new Date(this.addRideForm.controls.departureDate.value!.year,
                          this.addRideForm.controls.departureDate.value!.month -1,
                          this.addRideForm.controls.departureDate.value!.day,
                          this.addRideForm.controls.departureTime.value!.hour,
                          this.addRideForm.controls.departureTime.value!.minute);
    
    this.isLoading = true;
    const subscription = this.rideService.post(this.addRideForm.controls.pickupCity.value!, this.addRideForm.controls.dropOffCity.value!, startsAt, 
                        this.addRideForm.controls.rideDuration.value!, this.addRideForm.controls.numberOfProposedSeats.value!,
                        <Car>this.addRideForm.controls.car.value)
                      .subscribe({
                        next: () => {
                          this.isLoading = false;
                          this.toastService.showSuccess("Ride published successfully");
                          this.router.navigate(["/search"]);
                        },
                        error: (error) => {
                          this.isLoading = false; 
                          this.toastService.showDanger(error, "Fail to register user");
                        }
                      });
                      
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  goToPreviousStep() {
    if(this.currentStep() > Steps.pickupCity)
      this.currentStep.set(this.currentStep() -1);

    this.displayErrors = false;
  }

  goToNextStep() {
    if(this.currentStep() == Steps.publishRide || !this.isCurrentStepValid())
    {
      this.displayErrors = true;
      return;
    }
    
    this.displayErrors = false;
    this.currentStep.set(this.currentStep() +1);
  }

  selectedCarChanged(target:any|null) {
    if(!target || !target.value)
      return;

    let carId = target.value;
    let selectedCar = this.userCars().find(c => c.id == carId);
    
    this.addRideForm.controls.car.controls.brand.setValue(selectedCar!.brand);
    this.addRideForm.controls.car.controls.model.setValue(selectedCar!.model);
    this.addRideForm.controls.car.controls.colorHexCode.setValue(selectedCar!.colorHexCode);
    this.addRideForm.controls.car.controls.numberOfSeats.setValue(selectedCar!.numberOfSeats);
  }

  get isPickupCityInvalid()
  {
    return (
      this.addRideForm.controls.pickupCity.invalid
      && this.displayErrors
    );
  }

  get isDropOffCityInvalid()
  {
    return (
      this.addRideForm.controls.dropOffCity.invalid
      && this.displayErrors
    );
  }

  get isDepartureDateInvalid()
  {
    return (
      this.addRideForm.controls.departureDate.invalid
      && this.displayErrors
    );
  }

  get isDepartureTimeInvalid()
  {
    return (
      this.addRideForm.controls.departureTime.invalid
      && this.displayErrors
    );
  }

  get isRideDurationInvalid()
  {
    return (
      this.addRideForm.controls.rideDuration.invalid
      && this.displayErrors
    );
  }

  get isCarBrandInvalid()
  {
    return (
      this.addRideForm.controls.car.controls.brand.invalid
      && this.displayErrors
    );
  }

  get isCarModelInvalid()
  {
    return (
      this.addRideForm.controls.car.controls.model.invalid
      && this.displayErrors
    );
  }

  get isCarNumberOfSeatsInvalid()
  {
    return (
      this.addRideForm.controls.car.controls.numberOfSeats.invalid
      && this.displayErrors
    );
  }

  get isNumberOfProposedSeatsInvalid()
  {
    return (
      this.addRideForm.controls.numberOfProposedSeats.invalid
      && this.displayErrors
    );
  }

  // custom validator
  departureDateTimeInFuture(departureDate: NgbDateStruct): ValidatorFn {
  
        return (control: AbstractControl): ValidationErrors | null => {
            const value = <{ hour: number, minute: number }>control.value;
            if (!value)
                return null;

        let departureDateTime =  new Date(departureDate.year, departureDate.month -1, departureDate.day, value.hour, value.minute);

        let isDateTimeValid = departureDateTime.getTime() > (new Date()).getTime();

        return !isDateTimeValid ? {dateTimeNotInTheFuture: true}: null;
    }
  };
}

export enum Steps {
    askAuthentication = 1,
    pickupCity = 2,
    dropOffCity = 3,
    departureDate = 4,
    departureTime = 5,
    rideDuration = 6,
    car = 7,
    numberOfProposedSeats = 8,
    publishRide = 9
}
