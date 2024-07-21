import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account } from '../Models/account.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://localhost:7035/api/Admin';

  constructor(private http: HttpClient) { }

  getNotApprovedAccounts(): Observable<Account[]> {
    return this.http.get<Account[]>(`${this.apiUrl}/not-approved-accounts`);
  }

  getAllAccounts(): Observable<Account[]> {
    return this.http.get<Account[]>(`${this.apiUrl}/all-accounts`);
  }

  approveAccount(accountId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/approve-account/${accountId}`, {});
  }

  cancelAccount(accountId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/cancel-account/${accountId}`);
  }
}
