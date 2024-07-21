// user-dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  isAccountApproved!:boolean;
  openAccountButton!:boolean;
  toRegisterForBanking!:boolean;

  
constructor(private authService:AuthService,private accountService:AccountService,private router:Router,private route:ActivatedRoute){};

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.route.paramMap.subscribe(param => {
      this.accountService.getAccountUserById(this.userId).subscribe((resp) => {
        this.account = resp;
        console.log(this.account?.optForNetBanking);
        console.log(this.account?.transactionPassword);
        if(this.account?.isApproved){
          this.isAccountApproved = true;
        }else{
          this.isAccountApproved = false;
        }
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
    })
  }
  selectedAccount: any = null;

  openAccount(): void {
    this.openAccountButton = false;
    this.router.navigate(['user-dashboard/create-account']);
  }
}