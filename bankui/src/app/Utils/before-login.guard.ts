import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';
import { catchError, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BeforeLoginGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(): Observable<boolean> {
    if (this.authService.hasToken()) {
      return this.authService.loggedInUser$.pipe(
        tap((user:any) => console.log(user)),
        map((user: any) => {
          if (user.Role === 'Admin') {
            this.router.navigate(['/admin-dashboard']);
            return false;
          } else if (user.Role === 'User') {
            this.router.navigate(['/user-dashboard']);
            return false;
          }
          this.router.navigate(['/home']);
          return false;
        }),
        catchError(() => {
          this.router.navigate(['/home']);
          return of(false);
        })
      );
    } else {
      return of(true);
    }
  }
}
