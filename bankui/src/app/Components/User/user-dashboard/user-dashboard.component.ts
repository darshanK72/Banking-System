// user-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Account } from 'src/app/Models/account.model';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit{
  userId!:number;
  account!:any;
  openAccountButton!:boolean;
  toRegisterForBanking!:boolean;
  
constructor(private authService:AuthService,private accountService:AccountService,private router:Router){};

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.accountService.getAccountUserById(this.userId).subscribe((resp) => {
      this.account = resp;
      console.log(this.account?.optForNetBanking);
      console.log(this.account?.transactionPassword);
      if(this.account?.optForNetBanking && this.account?.transactionPassword != null){
        this.toRegisterForBanking = true;
      }else{
        this.toRegisterForBanking = false;
      }
      console.log(this.toRegisterForBanking);
      this.accountService.setAccount(this.account);
    })

    if(this.account == null){
      this.openAccountButton = true;
    }
  }
  activeComponent: string = 'account-statement';
  statementFrom: string = '';
  statementTo: string = '';
  accounts: any[] = [
    { accountNumber: '1234567890', name: 'John Doe', accountType: 'Savings', balance: 5000 }
  ];
  selectedAccount: any = null;

  setActiveComponent(component: string) {
    this.activeComponent = component;
  }
  openAccount(): void {
    this.openAccountButton = false;
    this.router.navigate(['user-dashboard/create-account']);
  }
}