import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/components/register/register.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { SearchRidesComponent } from './features/rides/components/search-rides/search-rides.component';
import { AddRideComponent } from './features/rides/components/add-ride/add-ride.component';

export const routes: Routes = [
  {
    path: '',
    component: SearchRidesComponent
  },
  {
    path: 'search', 
    component: SearchRidesComponent
  },
  {
    path: 'register', 
    component: RegisterComponent
  },
  {
    path: 'login', 
    component: LoginComponent
  },
  {
    path: 'addride', 
    component: AddRideComponent
  },
  {
    path: '**',
    component: SearchRidesComponent,
  },
];