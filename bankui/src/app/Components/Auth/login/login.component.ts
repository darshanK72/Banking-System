import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from 'src/app/Models/auth.dto';
import { AuthService } from 'src/app/Services/auth.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  isServerError:boolean = false;
  errorMessage:any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      usernameOrEmail: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.loginForm.invalid) {
      return;
    }

    const loginRequest: LoginRequest = this.loginForm.value;

    this.authService.login(loginRequest).subscribe({
      next: () => {
        var user = this.authService.getUserRoleAndId();
        if(user?.UserId){
          this.router.navigate(['/user-dashboard']);
        }else{
          this.router.navigate(['admin-dashboard'])
        }
      },
      error: (err) => {
        console.error('Login failed', err);
        this.isServerError = true;
        this.errorMessage = err.error;
      }
    });
  }
}
