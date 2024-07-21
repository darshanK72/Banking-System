// account-list.component.ts
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Account } from 'src/app/Models/account.model';
import { AdminService } from 'src/app/Services/admin.service';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {
  accounts: Account[] = [];
  selectedAccount!: any;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadNotApprovedAccounts();
  }

  loadNotApprovedAccounts(): void {
    this.adminService.getAllAccounts().subscribe(
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
}

