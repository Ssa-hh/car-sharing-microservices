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

  isLoading: boolean = false;

  constructor(private readonly authService: AuthService, private readonly destroyRef: DestroyRef, private readonly router: Router){}

  onSubmit() {
    this.isLoading = true;
    const subscription = this.authService.login(<string>this.signInForm.value.email, <string>this.signInForm.value.password)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.router.navigate(["/"])
        },
        error: (error:any) => { this.isLoading = false; console.log("Fail to sign in, error: ", error); }
      });

      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
  }
}
