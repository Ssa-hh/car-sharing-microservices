import { Component, DestroyRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  signUpForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });

  constructor(private readonly authService: AuthService, private readonly destroyRef: DestroyRef, private readonly router: Router){}

  onSubmit() {
    console.log("submit signin form");
    console.log("Form: ", this.signUpForm.value);
    const subscription = this.authService.register(<string>this.signUpForm.value.email, <string>this.signUpForm.value.password, 
      <string>this.signUpForm.value.firstName, <string>this.signUpForm.value.lastName)
      .subscribe({
        next: () => {
          console.log("Succesful sign up");
          this.router.navigate(["signin"])
        },
        error: (error:any) => console.log("Fail to sign up, error: ", error)
      });

      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
  }
}
