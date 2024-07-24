import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupComponent } from './Components/Auth/signup/signup.component';
import { AdminDashboardComponent } from './Components/Admin/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './Components/User/user-dashboard/user-dashboard.component';
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
import { AccountRequestsComponent } from './Components/Admin/account-requests/account-requests.component';
import { AuthGuard } from './Utils/auth.guard';
import { ApprovedAccountGuard } from './Utils/approved-account.guard';
import { NetBankingGuard } from './Utils/net-banking-enabled.guard';
import { NoNetBankingGuard } from './Utils/non-net-banking.guard';
import { AccountGuard } from './Utils/account.guard';
import { UserGuard } from './Utils/user.guard';
import { AdminGuard } from './Utils/admin.guard';
import { BeforeLoginGuard } from './Utils/before-login.guard';
import { RoleRedirectGuard } from './Utils/role-redirect.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [BeforeLoginGuard] },
  {
    path: 'signup',
    component: SignupComponent,
    canActivate: [BeforeLoginGuard],
  },
  {
    path: 'forget-username',
    component: ForgetUsernameComponent,
    canActivate: [BeforeLoginGuard],
  },
  {
    path: 'forget-password',
    component: ForgetPasswordComponent,
    canActivate: [BeforeLoginGuard],
  },
  {
    path: 'reset-password',
    component: PasswordResetComponent,
    canActivate: [BeforeLoginGuard],
  },
  {
    path: 'user-dashboard',
    component: UserDashboardComponent,
    canActivate: [AuthGuard, UserGuard],
    children: [
      { path: '', redirectTo: 'user-home', pathMatch: 'full' },
      { path: 'user-home', component: UserHomeComponent },
      {
        path: 'create-account',
        component: CreateAccountComponent,
        canActivate: [AuthGuard, UserGuard],
      },
      {
        path: 'register-netbanking',
        component: RegisterNetbankingComponent,
        canActivate: [
          AuthGuard,
          UserGuard,
          ApprovedAccountGuard,
          NoNetBankingGuard,
        ],
      },
      {
        path: 'account-statement',
        component: AccountStatementComponent,
        canActivate: [AuthGuard, UserGuard, ApprovedAccountGuard],
      },
      {
        path: 'account-summary',
        component: AccountSummaryComponent,
        canActivate: [AuthGuard, UserGuard, ApprovedAccountGuard],
      },
      {
        path: 'payee-list',
        component: PayeeListComponent,
        canActivate: [AuthGuard, UserGuard, ApprovedAccountGuard],
      },
      {
        path: 'funds-transfer',
        component: FundsTransferComponent,
        canActivate: [
          AuthGuard,
          UserGuard,
          ApprovedAccountGuard,
          NetBankingGuard,
        ],
      },
    ],
  },
  {
    path: 'admin-dashboard',
    component: AdminDashboardComponent,
    canActivate: [AuthGuard, AdminGuard],
    children: [
      { path: '', redirectTo: 'account-requests', pathMatch: 'full' },
      { path: 'account-requests', component: AccountRequestsComponent,canActivate:[AuthGuard, AdminGuard] },
      { path: 'account-list', component: AccountListComponent,canActivate:[AuthGuard, AdminGuard]},
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
