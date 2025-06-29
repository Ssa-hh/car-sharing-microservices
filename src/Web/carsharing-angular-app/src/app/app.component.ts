import { Component, OnInit } from '@angular/core';
import { AuthService } from './features/auth/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Carsharing';

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.authService.autoLogin();
  }
}
