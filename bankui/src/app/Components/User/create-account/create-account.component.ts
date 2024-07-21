import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { Account } from 'src/app/Models/account.model';
import { AccountService } from 'src/app/Services/account.service';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.css']
})
export class CreateAccountComponent implements OnInit {
  registrationForm!: FormGroup;
  userId!:number;
  isServerSuccess: boolean = false;
  isServerError: boolean = false;
  successMessage: any;
  errorMessage: any;

  constructor(private fb: FormBuilder, private accountService: AccountService,private authService:AuthService,private router:Router) { }

  ngOnInit(): void {

    this.userId = this.authService.getUserId()

    this.registrationForm = this.fb.group({
      title: ['', Validators.required],
      firstName: ['', Validators.required],
      middleName: [''],
      lastName: ['', Validators.required],
      fathersName: ['', Validators.required],
      mobileNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      emailId: ['', [Validators.email]],
      aadharCardNumber: ['', [Validators.required, Validators.pattern(/^\d{12}$/)]],
      dateOfBirth: ['', Validators.required],
      addressLine1: ['', Validators.required],
      addressLine2: [''],
      landmark: [''],
      state: ['', Validators.required],
      city: ['', Validators.required],
      pincode: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
      permanentAddressSameAsMailing: [false],
      permanentAddressLine1: ['', Validators.required],
      permanentAddressLine2: [''],
      permanentLandmark: [''],
      permanentState: ['', Validators.required],
      permanentCity: ['', Validators.required],
      permanentPincode: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
      occupationType: ['', Validators.required],
      sourceOfIncome: ['', Validators.required],
      grossAnnualIncome: ['', [Validators.required, Validators.min(0)]],
      wantDebitCard: [false],
      accountType:['',Validators.required],
      optForNetBanking: [false],
      agree: [false, Validators.requiredTrue]
    });
  }

  onPermanentAddressChange(): void {
    const sameAsMailingAddress = this.registrationForm.get('permanentAddressSameAsMailing')!.value;
    
    if (sameAsMailingAddress) {
      this.registrationForm.patchValue({
        permanentAddressLine1: this.registrationForm.get('addressLine1')!.value,
        permanentAddressLine2: this.registrationForm.get('addressLine2')!.value,
        permanentLandmark: this.registrationForm.get('landmark')!.value,
        permanentState: this.registrationForm.get('state')!.value,
        permanentCity: this.registrationForm.get('city')!.value,
        permanentPincode: this.registrationForm.get('pincode')!.value
      });

      // Disable permanent address fields
      Object.keys(this.registrationForm.controls).forEach(key => {
        if (key.startsWith('permanent') && key !== 'permanentAddressSameAsMailing') {
          this.registrationForm.get(key)!.disable();
        }
      });
    } else {
      // Clear and enable permanent address fields
      Object.keys(this.registrationForm.controls).forEach(key => {
        if (key.startsWith('permanent') && key !== 'permanentAddressSameAsMailing') {
          this.registrationForm.get(key)!.enable();
          this.registrationForm.get(key)!.setValue('');
        }
      });
    }
  }

  onSubmit() {
    console.log(this.registrationForm);
    if (this.registrationForm.valid) {
      const accountDTO: Account = {
        title: this.registrationForm.get('title')!.value,
        firstName: this.registrationForm.get('firstName')!.value,
        middleName: this.registrationForm.get('middleName')!.value,
        lastName: this.registrationForm.get('lastName')!.value,
        fathersName: this.registrationForm.get('fathersName')!.value,
        mobileNumber: this.registrationForm.get('mobileNumber')!.value,
        emailId: this.registrationForm.get('emailId')!.value,
        aadharCardNumber: this.registrationForm.get('aadharCardNumber')!.value,
        dateOfBirth: new Date(this.registrationForm.get('dateOfBirth')!.value),
        residentialAddress: {
          addressLine1: this.registrationForm.get('addressLine1')!.value,
          addressLine2: this.registrationForm.get('addressLine2')!.value,
          landmark: this.registrationForm.get('landmark')!.value,
          city: this.registrationForm.get('city')!.value,
          state: this.registrationForm.get('state')!.value,
          pincode: this.registrationForm.get('pincode')!.value
        },
        permanentAddress: {
          addressLine1: this.registrationForm.get('permanentAddressLine1')!.value,
          addressLine2: this.registrationForm.get('permanentAddressLine2')!.value,
          landmark: this.registrationForm.get('permanentLandmark')!.value,
          city: this.registrationForm.get('permanentCity')!.value,
          state: this.registrationForm.get('permanentState')!.value,
          pincode: this.registrationForm.get('permanentPincode')!.value
        },
        occupationType: this.registrationForm.get('occupationType')!.value,
        sourceOfIncome: this.registrationForm.get('sourceOfIncome')!.value,
        grossAnnualIncome: this.registrationForm.get('grossAnnualIncome')!.value,
        wantDebitCard: this.registrationForm.get('wantDebitCard')!.value,
        optForNetBanking: this.registrationForm.get('optForNetBanking')!.value,
        accountType:this.registrationForm.get('accountType')!.value,
        userId : this.userId
      };

      this.accountService.createAccount(accountDTO).subscribe(
        response => {
          this.isServerSuccess = true;
          this.isServerError = false;
          this.successMessage = response.message;
          console.log('Account created successfully:', response);
          this.registrationForm.reset();
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
      // Mark all fields as touched to trigger validation messages
      Object.keys(this.registrationForm.controls).forEach(key => {
        const control = this.registrationForm.get(key);
        control!.markAsTouched();
      });
    }
  }

  // Helper method to check if a field is invalid and touched
  isFieldInvalid(fieldName: string): boolean {
    const field = this.registrationForm.get(fieldName);
    return field!.invalid && (field!.dirty || field!.touched);
  }

  // Helper method to get error message for a field
  getErrorMessage(fieldName: string): string {
    const field = this.registrationForm.get(fieldName);
    if (field!.errors) {
      if (field!.errors['required']) {
        return `${this.getFieldLabel(fieldName)} is required.`;
      }
      if (field!.errors['email']) {
        return 'Invalid email address.';
      }
      if (field!.errors['pattern']) {
        return `Invalid ${this.getFieldLabel(fieldName)} format.`;
      }
      if (field!.errors['min']) {
        return `${this.getFieldLabel(fieldName)} must be greater than or equal to ${field!.errors['min'].min}.`;
      }
    }
    return '';
  }

  // Helper method to get a human-readable label for a field
  getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      title: 'Title',
      firstName: 'First Name',
      lastName: 'Last Name',
      fathersName: "Father's Name",
      mobileNumber: 'Mobile Number',
      emailId: 'Email ID',
      aadharCardNumber: 'Aadhar Card Number',
      dateOfBirth: 'Date of Birth',
      addressLine1: 'Address Line 1',
      state: 'State',
      city: 'City',
      pincode: 'Pincode',
      permanentAddressLine1: 'Permanent Address Line 1',
      permanentState: 'Permanent State',
      permanentCity: 'Permanent City',
      permanentPincode: 'Permanent Pincode',
      occupationType: 'Occupation Type',
      sourceOfIncome: 'Source of Income',
      grossAnnualIncome: 'Gross Annual Income',
      agree: 'Agreement'
    };
    return labels[fieldName] || fieldName;
  }
}
