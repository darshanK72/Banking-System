import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../Services/auth.service";
import { AccountService } from "../Services/account.service";
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AccountGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    const userId = this.authService.getUserId();
    return this.accountService.getAccountUserById(userId).pipe(
      map(account => {
        if (account != null) {
          return true;
        } else {
          this.router.navigate(['/user-dashboard']);
          return false;
        }
      }),
      catchError((error) => {
        this.router.navigate(['/user-dashboard']);
        return of(false);
      })
    );
  }
}
