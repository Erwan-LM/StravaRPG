import { Routes } from '@angular/router';
import { authGuard } from './auth/auth.guard';
import { AuthCallbackComponent } from './auth/auth-callback.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'auth/callback',
    component: AuthCallbackComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
