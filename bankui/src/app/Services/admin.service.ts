import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://localhost:7035/api';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get(`${this.apiUrl}/Users`);
  }

  updateUser(user: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Users/${user.id}`, user);
  }

  approveUser(userId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/Users/approve/${userId}`, {});
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Users/${userId}`);
  }
}
