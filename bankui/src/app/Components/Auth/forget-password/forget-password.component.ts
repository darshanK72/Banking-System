import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ForgetPasswordRequest, LoginRequest } from 'src/app/Models/auth.dto';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css'],
})
export class ForgetPasswordComponent implements OnInit {
  forgetForm!: FormGroup;
  isServerSuccess: boolean = false;
  isServerError: boolean = false;
  successMessage: any;
  errorMessage: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.forgetForm = this.fb.group({
      email: ['', Validators.required],
    });
  }

  forgetPassword() {
    if (this.forgetForm.invalid) {
      return;
    }

    const forgetPasswordRequest: ForgetPasswordRequest = this.forgetForm.value;

    this.authService.forgotPassword(forgetPasswordRequest).subscribe({
      next: (resp) => {
        console.log(resp);
        this.isServerSuccess = true;
        this.isServerError = false;
        this.successMessage = resp.message;
      },
      error: (err) => {
        this.isServerError = true;
        this.isServerSuccess = false;
        this.errorMessage = err.error.message ?? err.error;
      },
    });
  }
}
