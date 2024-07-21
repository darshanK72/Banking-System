import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {
  ForgetPasswordRequest,
  ForgetUsernameRequest,
} from 'src/app/Models/auth.dto';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-forget-username',
  templateUrl: './forget-username.component.html',
  styleUrls: ['./forget-username.component.css'],
})
export class ForgetUsernameComponent implements OnInit {
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

  forgetUsername() {
    if (this.forgetForm.invalid) {
      return;
    }

    const forgetUsernameRequest: ForgetUsernameRequest = this.forgetForm.value;

    this.authService.forgotUsername(forgetUsernameRequest).subscribe({
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
