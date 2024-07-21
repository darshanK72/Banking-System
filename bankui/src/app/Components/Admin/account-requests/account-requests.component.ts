import { Component, OnInit } from '@angular/core';
import { Account } from 'src/app/Models/account.model';
import { AdminService } from 'src/app/Services/admin.service';
@Component({
  selector: 'app-account-requests',
  templateUrl: './account-requests.component.html',
  styleUrls: ['./account-requests.component.css']
})
export class AccountRequestsComponent implements OnInit {
  accounts: Account[] = [];
  selectedAccount!: any;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadNotApprovedAccounts();
  }

  loadNotApprovedAccounts(): void {
    this.adminService.getNotApprovedAccounts().subscribe(
      (accounts: Account[]) => {
        this.accounts = accounts;
      },
      (error) => {
        console.error(error);
        window.alert('Error loading accounts: ' + error.message);
      }
    );
  }

  onView(account: Account): void {
    this.selectedAccount = account;
  }

  onApprove(accountId: number): void {
    this.adminService.approveAccount(accountId).subscribe(
      () => {
        this.loadNotApprovedAccounts();
        this.selectedAccount = null;
      },
      (error) => {
        window.alert('Error approving account: ' + error.message);
      }
    );
  }

  onCancel(accountId: number): void {
    this.adminService.cancelAccount(accountId).subscribe(
      () => {
        this.loadNotApprovedAccounts();
        this.selectedAccount = null;
      },
      (error) => {
        window.alert('Error canceling account: ' + error.message);
      }
    );
  }
}
