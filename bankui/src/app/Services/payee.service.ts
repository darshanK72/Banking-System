import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Payee } from '../Models/payee.model';

@Injectable({
  providedIn: 'root'
})
export class PayeeService {
  private apiUrl = "https://localhost:7035/api/Payees";

  constructor(private http: HttpClient) { }

  getPayees(): Observable<Payee[]> {
    return this.http.get<Payee[]>(this.apiUrl);
  }
  
  getPayeesByUserId(userId:number): Observable<Payee[]> {
    return this.http.get<Payee[]>(`${this.apiUrl}/user/${userId}`);
  }

  getPayee(id: number): Observable<Payee> {
    return this.http.get<Payee>(`${this.apiUrl}/${id}`);
  }

  createPayee(payee: Payee): Observable<Payee> {
    return this.http.post<Payee>(this.apiUrl, payee);
  }

  updatePayee(payee: Payee): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${payee.payeeId}`, payee);
  }

  deletePayee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
