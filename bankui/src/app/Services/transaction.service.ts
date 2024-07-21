import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Transaction } from '../Models/transaction.model';
import { TransactionResponse } from '../Models/transaction-response';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private apiUrl = "https://localhost:7035/api/Transactions";

  constructor(private http: HttpClient) { }

  getTransactions(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.apiUrl);
  }

  getTransaction(id: number): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.apiUrl}/${id}`);
  }

  getTransactionByUserId(userId: number): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/user/${userId}`);
  }

  transfer(transaction: Transaction): Observable<TransactionResponse> {
    return this.http.post<TransactionResponse>(`${this.apiUrl}/transfer`, transaction);
  }
}
