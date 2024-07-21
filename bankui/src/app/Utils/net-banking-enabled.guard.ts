import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable, map, catchError, of } from "rxjs";
import { AccountService } from "../Services/account.service";
import { AuthService } from "../Services/auth.service";

@Injectable({
  providedIn: 'root'
})
export class NetBankingGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private router: Router
  ) {}

  canActivate(): Observable<boolean> {
    const userId = this.authService.getUserId();
    return this.accountService.getAccountUserById(userId).pipe(
      map(account => {
        if (account.isApproved && account.optForNetBanking && account.transactionPassword != null) {
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
