import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { DecodedToken, LoginRequest } from '../Models/auth.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `https://localhost:7035/api/Auth`;

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.hasToken());
  private loggedInUser: BehaviorSubject<any> = new BehaviorSubject<any>(this.getUserDetails());

  constructor(private http: HttpClient,private router:Router) { }

  public get isLoggedIn$(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  public get loggedInUser$(): Observable<any> {
    return this.loggedInUser.asObservable();
  }

  public hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  public getUserDetails(): any {
    const userData = localStorage.getItem('user');
    if (userData) {
      return JSON.parse(userData);
    }
    return null;
  }

  public getUserId(){
    const userData = localStorage.getItem('user');
    if (userData) {
      let user = JSON.parse(userData);
        return user.UserId;
     
    }
    return null;
  }

  getUserRoleAndId(){
    const userData = localStorage.getItem('user');
    if (userData) {
      let user = JSON.parse(userData);
      let {AdminId,UserId,WasherId,Role} = user;
      if(AdminId) return {AdminId,Role};
      else if(UserId) return {UserId,Role};
    }
    return null;
  }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }

  registerAdmin(admin: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register/admin`, admin);
  }

  login(credentials:LoginRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        const token = response.token;
        if (token) {
          localStorage.setItem('token', token);
          this.loggedIn.next(true);
          const decodedToken = jwtDecode<DecodedToken>(token);
          console.log(decodedToken);
          const user = JSON.parse(decodedToken.user);
          this.loggedInUser.next(user);
          localStorage.setItem('user', JSON.stringify(user));
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.loggedIn.next(false);
    this.loggedInUser.next(null);
    this.router.navigate(['/login']);
  }


  resetPassword(resetRequest: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/reset-password`, resetRequest);
  }

  forgotUsername(emailRequest: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/forgot-username`, emailRequest);
  }

  forgotPassword(emailRequest: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/forgot-password`, emailRequest);
  }
}
