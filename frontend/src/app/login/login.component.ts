import { Component, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private readonly authService = inject(AuthService);

  login(): void {
    this.authService.loginWithGoogle();
  }
}
