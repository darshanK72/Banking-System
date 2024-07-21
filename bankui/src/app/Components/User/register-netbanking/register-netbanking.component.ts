import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NetBankingRequest } from 'src/app/Models/netbanking-request';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-register-netbanking',
  templateUrl: './register-netbanking.component.html',
  styleUrls: ['./register-netbanking.component.css']
})
export class RegisterNetbankingComponent implements OnInit{
  registerForm: FormGroup;
  userId!:number;
  otpMessage!:any;
  isServerSuccess: boolean = false;
  isServerError: boolean = false;
  successMessage: any;
  errorMessage: any;

  constructor(private fb: FormBuilder,private authService:AuthService,private accountService:AccountService,private router:Router) {
    this.registerForm = this.fb.group({
      accountNumber: ['', Validators.required],
      loginPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmLoginPassword: ['', Validators.required],
      transactionPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmTransactionPassword: ['', Validators.required],
      otp: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]]
    }, { validator: this.checkPasswords });
  }

  ngOnInit(): void {
      this.userId = this.authService.getUserId();
  }
  checkPasswords(group: FormGroup) {
    const loginPass = group.get('loginPassword')?.value;
    const confirmLoginPass = group.get('confirmLoginPassword')?.value;
    const transactionPass = group.get('transactionPassword')?.value;
    const confirmTransactionPass = group.get('confirmTransactionPassword')?.value;

    return loginPass === confirmLoginPass && transactionPass === confirmTransactionPass
      ? null : { notSame: true };
  }

  onSubmit() {
    console.log(this.registerForm);
    if (this.registerForm.valid) {
      const registerRequest:NetBankingRequest = {
        accountNumber: this.registerForm.get('accountNumber')!.value,
        userPassword: this.registerForm.get('loginPassword')!.value,
        transactionPassword: this.registerForm.get('transactionPassword')!.value,
        otp: this.registerForm.get('otp')!.value,
        userId:this.userId,
      }
      this.accountService.registerForNetbanking(registerRequest).subscribe(
        response => {
          this.isServerSuccess = true;
          this.isServerError = false;
          this.successMessage = response.message;
          console.log('Account created successfully:', response);
          this.registerForm.reset();
          this.router.navigate(['/user-dashboard']);
        },
        error => {
          this.isServerError = true;
          this.isServerSuccess = false;
          this.errorMessage = error.error.message ?? error.error;
          console.error('Error creating account:', error);
        }
      );
    } else {
      Object.keys(this.registerForm.controls).forEach(key => {
        const control = this.registerForm.get(key);
        control!.markAsTouched();
      });
    }
  }

  requestForOtp(){
    console.log("request for opt");
    this.accountService.requestOfOTP(this.userId).subscribe(resp => {
      this.otpMessage = resp.message;
    },error => {
      this.otpMessage = error.error.message ?? error.error;
    })
  }
}
