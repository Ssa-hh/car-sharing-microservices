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

  isLoading: boolean = false;

  constructor(private readonly authService: AuthService, private readonly destroyRef: DestroyRef, private readonly router: Router){}

  onSubmit() {
    this.isLoading = true;
    const subscription = this.authService.register(<string>this.signUpForm.value.email, <string>this.signUpForm.value.password, 
      <string>this.signUpForm.value.firstName, <string>this.signUpForm.value.lastName)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(["login"])
        },
        error: (error:any) => {this.isLoading = false; console.log("Fail to sign up, error: ", error);}
      });

      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
  }
}
