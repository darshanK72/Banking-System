import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Transaction } from '../Models/transaction.model';
import { TransactionResponse } from '../Models/transaction-response';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private apiUrl = 'https://localhost:7035/api/Transactions';

  constructor(private http: HttpClient) {}

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
    return this.http.post<TransactionResponse>(
      `${this.apiUrl}/transfer`,
      transaction
    );
  }

  searchTransactions(
    fromAccountNumber?: string,
    toAccountNumber?: number,
    minAmount?: string,
    maxAmount?: number,
    startDate?: string,
    endDate?: string,
    status?: string
  ): Observable<Transaction[]> {
    let params: any = {};

    if (fromAccountNumber !== undefined) params.fromAccountNumber = fromAccountNumber;
    if (toAccountNumber !== undefined) params.toAccountNumber = toAccountNumber;
    if (minAmount !== undefined) params.minAmount = minAmount;
    if (maxAmount !== undefined) params.maxAmount = maxAmount;
    if (startDate !== undefined) params.startDate = startDate;
    if (endDate !== undefined) params.endDate = endDate;
    if (status !== undefined) params.status = status;

    return this.http.get<Transaction[]>(`${this.apiUrl}/search`, { params });
  }
}
