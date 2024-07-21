import { Component, OnInit } from '@angular/core';
import { Account } from 'src/app/Models/account.model';
import { Transaction } from 'src/app/Models/transaction.model';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';
import { TransactionService } from 'src/app/Services/transaction.service';

@Component({
  selector: 'app-account-summary',
  templateUrl: './account-summary.component.html',
  styleUrls: ['./account-summary.component.css']
})
export class AccountSummaryComponent implements OnInit {
  account!: Account;
  transactions:Transaction[] = [];
  userId!:number;

  constructor(private accountService: AccountService,private authService:AuthService,private transasctionService:TransactionService) { }

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.loadAccountSummary();
  }

  loadAccountSummary(): void {
    this.accountService.getAccountUserById(this.userId).subscribe(
      (account: Account) => {
        console.log(account);
        this.account = account;
      },
      error => {
        console.error('Error loading account summary', error);
      }
    );

    this.transasctionService.getTransactionByUserId(this.userId).subscribe(trans => {
      console.log(trans);
      this.transactions = trans;
    },
    error => {
      console.error('Error loading account summary', error);
    })
  }
}
