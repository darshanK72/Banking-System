import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupComponent } from './Components/Auth/signup/signup.component';
import { AdminDashboardComponent } from './Components/Admin/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './Components/User/user-dashboard/user-dashboard.component';
import { AuthGuard } from './Utils/auth.guard';
import { RoleGuard } from './Utils/role.guard';
import { CreateAccountComponent } from './Components/User/create-account/create-account.component';
import { AccountListComponent } from './Components/Admin/account-list/account-list.component';
import { LoginComponent } from './Components/Auth/login/login.component';
import { HomeComponent } from './Layout/home/home.component';
import { ForgetUsernameComponent } from './Components/Auth/forget-username/forget-username.component';
import { ForgetPasswordComponent } from './Components/Auth/forget-password/forget-password.component';
import { PasswordResetComponent } from './Components/Auth/password-reset/password-reset.component';
import { RegisterNetbankingComponent } from './Components/User/register-netbanking/register-netbanking.component';
import { AccountStatementComponent } from './Components/User/account-statement/account-statement.component';
import { AccountSummaryComponent } from './Components/User/account-summary/account-summary.component';
import { FundsTransferComponent } from './Components/User/funds-transfer/funds-transfer.component';
import { UserHomeComponent } from './Components/User/user-home/user-home.component';
import { PayeeListComponent } from './Components/User/payee-list/payee-list.component';


const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'forget-username', component: ForgetUsernameComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'reset-password', component: PasswordResetComponent },
  { path: 'home', component: HomeComponent },
  { path: 'admin-dashboard', component: AdminDashboardComponent, canActivate: [AuthGuard, RoleGuard], data: { role: 'Admin' } },
  { path: 'user-dashboard',
    component: UserDashboardComponent,
    children: [
      { path: '', redirectTo: 'user-home', pathMatch: 'full' },  // Redirect empty path to 'user-home'
      { path: 'user-home', component: UserHomeComponent },
      { path: 'create-account', component: CreateAccountComponent },
      { path: 'register-netbanking', component: RegisterNetbankingComponent },
      { path: 'account-statement', component: AccountStatementComponent },
      { path: 'account-summary', component: AccountSummaryComponent },
      { path: 'payee-list', component: PayeeListComponent },
      { path: 'funds-transfer', component: FundsTransferComponent }
    ]
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },

  { path: '', redirectTo: '/payees', pathMatch: 'full' },
  { path: 'account-list', component: AccountListComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
