import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7035/api/';

  constructor(private http: HttpClient) {}

  getUserById(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}Users/${userId}`);
  }

  getUserWithTransactions(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}User/UserWithTransactions/${userId}`);
  }

  getUserWithPayees(userId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}User/UserWithPayees/${userId}`);
  }
  
  deletePayeesByUserId(userId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}Payees/${userId}`);
  }

  deleteTransaction(userId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}Transaction/DeleteTransaction/${userId}`);
  }
}
