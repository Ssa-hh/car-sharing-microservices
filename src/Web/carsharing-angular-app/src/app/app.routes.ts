import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/components/register/register.component';

export const routes: Routes = [
  {
    path: '',
    component: RegisterComponent
  },
  {
    path: 'register', 
    component: RegisterComponent
  },
  {
    path: '**',
    component: RegisterComponent,
  },
];