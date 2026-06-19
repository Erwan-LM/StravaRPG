import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from './user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenKey = 'strava_rpg_token';
  readonly apiUrl = 'http://localhost:5208';

  get token(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  get isAuthenticated(): boolean {
    return Boolean(this.token);
  }

  loginWithGoogle(): void {
    window.location.href = `${this.apiUrl}/auth/google`;
  }

  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/auth/me`);
  }
}
