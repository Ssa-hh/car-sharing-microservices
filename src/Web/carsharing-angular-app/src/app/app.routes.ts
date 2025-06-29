import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/components/register/register.component';
import { LoginComponent } from './features/auth/components/login/login.component';
import { SearchRidesComponent } from './features/rides/components/search-rides/search-rides.component';
import { AddRideComponent } from './features/rides/components/add-ride/add-ride.component';
import { AddUserCarComponent } from './features/auth/components/add-user-car/add-user-car.component';
import { AuthGuard } from './features/auth/services/auth-guard';

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
    path: 'addusercar',
    component: AddUserCarComponent, // Assuming this is the component for adding a car
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    component: SearchRidesComponent,
  },
];