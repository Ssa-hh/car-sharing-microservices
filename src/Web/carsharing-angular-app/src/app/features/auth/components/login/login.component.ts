import { Component, DestroyRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../shared/toast/toast.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  signInForm = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  isLoading: boolean = false;

  constructor(private readonly authService: AuthService, private readonly destroyRef: DestroyRef, 
    private readonly router: Router, private readonly toastService: ToastService,
    private route: ActivatedRoute){}

  onSubmit() {
    this.isLoading = true;
    const subscription = this.authService.login(<string>this.signInForm.value.email, <string>this.signInForm.value.password)
      .subscribe({
        next: () => {
          const returnUrl = this.route.snapshot.queryParamMap.get("returnUrl") || '/';
          this.isLoading = false;
          this.router.navigate([returnUrl])
        },
        error: (error:any) => { 
          this.isLoading = false; 
          this.toastService.showDanger(error); 
        }
      });

      this.destroyRef.onDestroy(() => {
        subscription.unsubscribe();
      });
  }

  get email() {
    return this.signInForm.controls.email;
  }

  get emailIsInvalid() {
    return (
      this.signInForm.controls.email.touched &&
      this.signInForm.controls.email.dirty &&
      this.signInForm.controls.email.invalid
    );
  }

  get passwordIsInvalid() {
    return (
      this.signInForm.controls.password.touched &&
      this.signInForm.controls.password.dirty &&
      this.signInForm.controls.password.invalid
    );
  }
}
