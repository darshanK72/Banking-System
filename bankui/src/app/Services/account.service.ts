import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Account } from '../Models/account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl = 'https://localhost:7035/api/Account';

  private userAccount = new BehaviorSubject<any>(null);
  account$ = this.userAccount.asObservable();

  setAccount(account: any) {
    this.userAccount.next(account);
  }

  constructor(private http: HttpClient) { }

  getAllAccounts(): Observable<Account[]> {
    return this.http.get<Account[]>(this.baseUrl);
  }

  requestOfOTP(userId:any):Observable<any>{
    return this.http.get<any>(`${this.baseUrl}/otp/${userId}`);
  }

  getAccountById(id: number): Observable<Account> {
    return this.http.get<Account>(`${this.baseUrl}/${id}`);
  }

  getAccountUserById(userId: number): Observable<Account> {
    return this.http.get<Account>(`${this.baseUrl}/user/${userId}`);
  }

  createAccount(account: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, account);
  }

  registerForNetbanking(request: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/register`, request);
  }

  updateAccount(id: number, account: Account): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, account);
  }

  deleteAccount(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
