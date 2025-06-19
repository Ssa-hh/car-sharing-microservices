import { Component, DestroyRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  signInForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  constructor(private readonly authService: AuthService, private readonly destroyRef: DestroyRef, private readonly router: Router){}

  onSubmit() {
    console.log("Form: ", this.signInForm.value);
    const subscription = this.authService.login(<string>this.signInForm.value.email, <string>this.signInForm.value.password)
      .subscribe({
        next: () => {
          console.log("Successful sign up");
          this.router.navigate(["/"])
        },
        error: (error:any) => console.log("Fail to sign in, error: ", error)
      });

      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
  }
}
