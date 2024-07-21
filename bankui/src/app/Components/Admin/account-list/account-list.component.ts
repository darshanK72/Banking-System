// account-list.component.ts
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {
  accounts: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getAccounts();
  }

  getAccounts() {
    this.http.get('https://localhost:7035/api/Accounts').subscribe({
      next: (response: any) => {
        this.accounts = response;
      },
      error: (error) => {
        console.error('Error fetching accounts', error);
      }
    });
  }

  deleteAccount(id: number) {
    this.http.delete(`https://localhost:7035/api/Accounts/${id}`).subscribe({
      next: (response) => {
        console.log('Account deleted successfully', response);
        this.getAccounts();
      },
      error: (error) => {
        console.error('Error deleting account', error);
      }
    });
  }
}
