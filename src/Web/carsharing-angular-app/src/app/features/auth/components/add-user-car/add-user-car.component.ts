import { Component, DestroyRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastService } from '../../../../shared/toast/toast.service';

@Component({
  selector: 'app-add-user-car',
  standalone: false,
  templateUrl: './add-user-car.component.html',
  styleUrl: './add-user-car.component.css'
})
export class AddUserCarComponent {
  isLoading = false;
  addCarForm = new FormGroup({
        brand: new FormControl('', Validators.required),
        model: new FormControl('', Validators.required),
        numberOfSeats: new FormControl(2, [Validators.required, Validators.min(2)]),
        colorHexCode: new FormControl<string|null|undefined>('')
      })
  
  constructor(private readonly authService: AuthService, private readonly router: Router, 
          private readonly toastService: ToastService, private readonly destroyRef: DestroyRef) { }
  
  addCar() {
    if(this.addCarForm.invalid) {
      this.addCarForm.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    
    const subscription = this.authService.addCar(
      this.addCarForm.value.brand!,
      this.addCarForm.value.model!,
      this.addCarForm.value.numberOfSeats!,
      this.addCarForm.value.colorHexCode ?? null
    ).subscribe({
      next: () => {
        this.isLoading = false;
        this.toastService.showSuccess('Car added successfully');
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Error adding car:', error);
        this.isLoading = false;
        this.toastService.showDanger(error, 'Failed to add car');
      }
    })

    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  get isCarBrandInvalid()
  {
    return (
      this.addCarForm.controls.brand.invalid
      && this.addCarForm.controls.brand.touched
      && this.addCarForm.controls.brand.dirty
    );
  }

  get isCarModelInvalid()
  {
    return (
      this.addCarForm.controls.model.invalid
      && this.addCarForm.controls.model.touched
      && this.addCarForm.controls.model.dirty
    );
  }

  get isCarNumberOfSeatsInvalid()
  {
    return (
      this.addCarForm.controls.numberOfSeats.invalid
      && this.addCarForm.controls.numberOfSeats.touched
      && this.addCarForm.controls.numberOfSeats.dirty
    );
  }
}
