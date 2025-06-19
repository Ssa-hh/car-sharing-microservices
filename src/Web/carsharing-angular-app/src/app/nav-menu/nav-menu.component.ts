import { Component, OnChanges, OnInit, DestroyRef } from '@angular/core';
import { AuthService } from '../features/auth/services/auth.service';

@Component({
  selector: 'app-nav-menu',
  standalone: false,
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.css'
})
export class NavMenuComponent implements OnInit {
  toggleNavbar = true;
  isAuthenticated = false;
  userFirstName = '';
  userLastName = '';

  constructor(private authService: AuthService, private destroyRef: DestroyRef) {}

  ngOnInit(): void {
    let subscription = this.authService.user.subscribe(user => {
      this.isAuthenticated = !!user;
      this.userFirstName = user ? user.firstName : '';
      this.userLastName = user ? user.lastName : '';
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  logout() {
    this.authService.logout();
  }
}
