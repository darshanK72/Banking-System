import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../Services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleRedirectGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> {
    if (this.authService.hasToken()) {
      return this.authService.getUserDetails().pipe(
        map((user:any) => {
          if (user.Role === 'Admin') {
            this.router.navigate(['/admin-dashboard']);
          } else if (user.Role === 'User') {
            this.router.navigate(['/user-dashboard']);
          } else {
            this.router.navigate(['/login']); // Redirect to login if role is not recognized
          }
          return false; // Prevent access to the root path
        }),
        catchError(() => {
          this.router.navigate(['/login']);
          return of(false);
        })
      );
    } else {
      this.router.navigate(['/login']);
      return of(false);
    }
  }
}
