import { Component, OnInit } from '@angular/core';
import { Account } from 'src/app/Models/account.model';
import { Transaction } from 'src/app/Models/transaction.model';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';
import { TransactionService } from 'src/app/Services/transaction.service';

@Component({
  selector: 'app-account-statement',
  templateUrl: './account-statement.component.html',
  styleUrls: ['./account-statement.component.css']
})
export class AccountStatementComponent implements OnInit {
  account!: Account;
  transactions: Transaction[] = [];
  userId!: number;
  selectedTransaction?: Transaction;

  constructor(
    private accountService: AccountService,
    private authService: AuthService,
    private transactionService: TransactionService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.loadAccountSummary();
  }

  loadAccountSummary(): void {
    this.accountService.getAccountUserById(this.userId).subscribe(
      (account: Account) => {
        this.account = account;
      },
      error => {
        console.error('Error loading account summary', error);
      }
    );

    this.transactionService.getTransactionByUserId(this.userId).subscribe(
      (transactions: Transaction[]) => {
        this.transactions = transactions;
      },
      error => {
        console.error('Error loading transactions', error);
      }
    );
  }

  viewTransactionDetails(transaction: Transaction): void {
    this.selectedTransaction = transaction;
  }

  isIncoming(transaction: Transaction): boolean {
    return transaction.toAccountId === this.account.accountId;
  }
}
