import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ForgetPasswordRequest,
  ForgetUsernameRequest,
  PasswordResetRequest,
} from 'src/app/Models/auth.dto';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css'],
})
export class PasswordResetComponent implements OnInit {
  passwordResetForm!: FormGroup;
  token: any;
  isServerSuccess: boolean = false;
  isServerError:boolean = false;
  successMessage: any;
  errorMessage:any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.passwordResetForm = this.fb.group({
      password: ['', Validators.required],
      verifyPassword: ['', Validators.required],
      token: ['', Validators.required],
    });
  
    this.route.queryParamMap.subscribe((params) => {
      this.token = params.get('token');
      console.log(this.token);
      this.passwordResetForm.patchValue({ token: this.token });
    });
  }

  resetPassword() {
    if (this.passwordResetForm.invalid) {
      return;
    }

    const passwordResetRequest: PasswordResetRequest =
      this.passwordResetForm.value;

    if (passwordResetRequest.password != passwordResetRequest.verifyPassword) {
      this.isServerError = true;
      this.errorMessage = 'Password Should Match';
    } else {
      this.authService.resetPassword(passwordResetRequest).subscribe({
        next: (resp) => {
          console.log(resp);
          console.log(resp);
          this.isServerSuccess = true;
          this.isServerError = false
          this.successMessage = resp.message;
        },
        error: (err) => {
          console.log(err);
          this.isServerError = true;
          this.isServerSuccess = false;
          this.errorMessage = err.error.message ?? err.error;
        },
      });
    }
  }
}
