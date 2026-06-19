import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  template: `
    <main class="flex min-h-screen items-center justify-center px-6">
      <div class="rounded-lg border border-zinc-200 bg-white p-6 text-center shadow-sm">
        <p class="text-sm font-medium text-zinc-500">Signing you in</p>
        <h1 class="mt-2 text-xl font-semibold text-zinc-950">Strava RPG</h1>
      </div>
    </main>
  `
})
export class AuthCallbackComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.router.navigateByUrl('/login');
      return;
    }

    this.authService.saveToken(token);
    this.router.navigateByUrl('/dashboard');
  }
}
