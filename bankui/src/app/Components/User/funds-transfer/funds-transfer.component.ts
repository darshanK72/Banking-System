import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Account } from 'src/app/Models/account.model';
import { Payee } from 'src/app/Models/payee.model';
import { TransactionResponse } from 'src/app/Models/transaction-response';
import { Transaction } from 'src/app/Models/transaction.model';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';
import { PayeeService } from 'src/app/Services/payee.service';
import { TransactionService } from 'src/app/Services/transaction.service';

@Component({
  selector: 'app-funds-transfer',
  templateUrl: './funds-transfer.component.html',
  styleUrls: ['./funds-transfer.component.css']
})
export class FundsTransferComponent implements OnInit {
  transferForm: FormGroup;
  paymentTypes = ['NEFT', 'RTGS', 'IMPS'];
  selectedPaymentType = 'NEFT';
  payees: Payee[] = [];
  accounts: Account[] = [];
  userId!: number;

  constructor(
    private fb: FormBuilder,
    private payeeService: PayeeService,
    private accountService: AccountService,
    private authService: AuthService,
    private transactionService: TransactionService,
    private router: Router
  ) {
    this.transferForm = this.fb.group({
      fromAccount: ['', Validators.required],
      toAccount: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0)]],
      transactionDate: ['', Validators.required],
      maturityInstructions: [null],
      remark: [null],
      transactionType: [this.selectedPaymentType, Validators.required] // Add transactionType field
    });
  }

  ngOnInit() {
    this.userId = this.authService.getUserId();
    this.loadPayees();
    this.loadAccounts();
  }

  loadPayees(): void {
    this.payeeService.getPayees().subscribe(
      (payees: Payee[]) => {
        this.payees = payees;
        console.log(payees);
      },
      (error) => {
        console.log(error);
        window.alert('Error loading beneficiaries: ' + error.message);
      }
    );
  }

  loadAccounts() {
    this.accountService.getAccountUserById(this.userId).subscribe(
      account => {
        this.accounts.push(account);
      },
      error => console.error('Error loading accounts', error)
    );
  }

  onPaymentTypeChange(event: any) {
    this.selectedPaymentType = event.target.value;
    this.transferForm.get('transactionType')?.setValue(this.selectedPaymentType); // Update the transactionType field
    if (this.selectedPaymentType === 'NEFT') {
      this.transferForm.get('maturityInstructions')?.disable();
    } else {
      this.transferForm.get('maturityInstructions')?.enable();
    }
  }

  onSubmit() {
    if (this.transferForm.valid) {
      const transaction: Transaction = {
        transactionId: 0,
        description: this.transferForm.value.remark,
        amount: this.transferForm.value.amount,
        transactionDate: new Date(this.transferForm.value.transactionDate),
        fromAccountId: 0, // Handle account ID
        fromAccountNumber: this.transferForm.value.fromAccount,
        toAccountId: 0, // Handle account ID
        toAccountNumber: this.transferForm.value.toAccount,
        transactionType: this.transferForm.value.transactionType,
        status: '',
        remark: this.transferForm.value.remark,
        maturityInstructions: this.transferForm.value.maturityInstructions
      };

      this.transactionService.transfer(transaction).subscribe(
        (response: TransactionResponse) => {
          if (response.success) {
            window.alert('Transaction successful: ' + response.message);
            this.router.navigate(['/user-dashboard']);
            this.transferForm.reset();
          } else {
            window.alert('Transaction failed: ' + response.message);
          }
        },
        error => {
          console.error('Transaction failed:', error);
          window.alert('Transaction failed: ' + error.message);
        }
      );
    }
  }

  addNewPayee() {
    this.router.navigate(['user-dashboard/payee-list']);
  }
}
