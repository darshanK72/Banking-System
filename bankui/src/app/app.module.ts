import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignupComponent } from './Components/Auth/signup/signup.component';
import { AdminDashboardComponent } from './Components/Admin/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './Components/User/user-dashboard/user-dashboard.component';
import { CreateAccountComponent } from './Components/User/create-account/create-account.component';
import { AccountListComponent } from './Components/Admin/account-list/account-list.component';
import { LoginComponent } from './Components/Auth/login/login.component';
import { HeaderComponent } from './Layout/header/header.component';
import { FooterComponent } from './Layout/footer/footer.component';
import { HomeComponent } from './Layout/home/home.component';
import { ForgetUsernameComponent } from './Components/Auth/forget-username/forget-username.component';
import { ForgetPasswordComponent } from './Components/Auth/forget-password/forget-password.component';
import { PasswordResetComponent } from './Components/Auth/password-reset/password-reset.component';
import { FundsTransferComponent } from './Components/User/funds-transfer/funds-transfer.component';
import { AccountStatementComponent } from './Components/User/account-statement/account-statement.component';
import { AccountSummaryComponent } from './Components/User/account-summary/account-summary.component';
import { RegisterNetbankingComponent } from './Components/User/register-netbanking/register-netbanking.component';
import { UserHomeComponent } from './Components/User/user-home/user-home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PayeeListComponent } from './Components/User/payee-list/payee-list.component';
import { JwtInterceptor } from './Utils/jwt.interceptor';
import { CommonModule } from '@angular/common';
import { AccountRequestsComponent } from './Components/Admin/account-requests/account-requests.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    AdminDashboardComponent,
    UserDashboardComponent,
    CreateAccountComponent,
    AccountListComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    ForgetUsernameComponent,
    ForgetPasswordComponent,
    PasswordResetComponent,
    FundsTransferComponent,
    AccountStatementComponent,
    AccountSummaryComponent,
    RegisterNetbankingComponent,
    UserHomeComponent,
    PayeeListComponent,
    AccountRequestsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
