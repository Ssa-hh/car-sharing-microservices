import { Component, DestroyRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../shared/toast/toast.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  signUpForm = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.required,Validators.minLength(6)]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
  });

  isLoading: boolean = false;

  constructor(private readonly authService: AuthService, 
    private readonly destroyRef: DestroyRef,
    private readonly router: Router,
    private readonly toastService: ToastService,
    private route: ActivatedRoute){}

  onSubmit() {
    this.isLoading = true;
    let loginSubscription: Subscription
    const registerSubscription = this.authService.register(<string>this.signUpForm.value.email, <string>this.signUpForm.value.password, 
      <string>this.signUpForm.value.firstName, <string>this.signUpForm.value.lastName)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.toastService.showSuccess("User registered successfully");
          loginSubscription = this.authService.login(<string>this.signUpForm.value.email, <string>this.signUpForm.value.password)
            .subscribe({
              next: () => {
                const returnUrl = this.route.snapshot.queryParamMap.get("returnUrl") || '/';
                this.isLoading = false;
                this.router.navigate([returnUrl])
              },
              error: (error:any) => { 
                this.isLoading = false; 
                this.toastService.showDanger(error, "Fail to login user");
                this.router.navigate(["/user/login"]); 
              }
            })
        },
        error: (error:any) => {
          this.isLoading = false; 
          this.toastService.showDanger(error, "Fail to register user");
        }
      });
      
      this.destroyRef.onDestroy(() => {
        registerSubscription.unsubscribe();

        if(loginSubscription)
          loginSubscription.unsubscribe();
      });
  }

  get email() {
    return this.signUpForm.controls.email;
  }

  get password() {
    return this.signUpForm.controls.password;
  }

  get emailIsInvalid() {
    return (
      this.signUpForm.controls.email.touched &&
      this.signUpForm.controls.email.dirty &&
      this.signUpForm.controls.email.invalid
    );
  }

  get passwordIsInvalid() {
    return (
      this.signUpForm.controls.password.touched &&
      this.signUpForm.controls.password.dirty &&
      this.signUpForm.controls.password.invalid
    );
  }

  get firstNameIsInvalid() {
    return (
      this.signUpForm.controls.firstName.touched &&
      this.signUpForm.controls.firstName.dirty &&
      this.signUpForm.controls.firstName.invalid
    );
  }

  get lastNameIsInvalid() {
    return (
      this.signUpForm.controls.lastName.touched &&
      this.signUpForm.controls.lastName.dirty &&
      this.signUpForm.controls.lastName.invalid
    );
  }
}
