import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { User } from '../auth/user.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  protected user: User | null = null;
  protected isLoading = true;

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        this.user = user;
        this.isLoading = false;
      },
      error: () => {
        this.authService.logout();
        this.router.navigateByUrl('/login');
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }
}
