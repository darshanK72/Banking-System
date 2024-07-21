import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Payee } from 'src/app/Models/payee.model';
import { AuthService } from 'src/app/Services/auth.service';
import { PayeeService } from 'src/app/Services/payee.service';

@Component({
  selector: 'app-payee-list',
  templateUrl: './payee-list.component.html',
  styleUrls: ['./payee-list.component.css']
})
export class PayeeListComponent implements OnInit {
  payeeForm: FormGroup;
  payees: Payee[] = [];
  editMode: boolean = false;
  selectedPayeeId: number | null = null;
  userId!:number;

  constructor(
    private fb: FormBuilder,
    private payeeService: PayeeService,
    private authService:AuthService
  ) {
    this.payeeForm = this.fb.group({
      beneficiaryName: ['', [Validators.required, Validators.maxLength(100)]],
      beneficiaryAccountNumber: ['', [Validators.required, Validators.maxLength(20)]],
      reEnterAccountNumber: ['', [Validators.required, Validators.maxLength(20)]],
    }, { validator: this.accountNumberMatchValidator });
  }

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.loadPayees();
  }

  accountNumberMatchValidator(group: FormGroup) {
    const accountNumber = group.get('beneficiaryAccountNumber')?.value;
    const reEnterAccountNumber = group.get('reEnterAccountNumber')?.value;
    return accountNumber === reEnterAccountNumber ? null : { mismatch: true };
  }

  loadPayees(): void {
    this.payeeService.getPayeesByUserId(this.userId).subscribe(
      (payees: Payee[]) => {
        this.payees = payees;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading beneficiaries: ' + error.message);
      }
    );
  }

  onSave(): void {
    if (this.payeeForm.valid) {
      const payee: Payee = {
        payeeId: this.selectedPayeeId ? this.selectedPayeeId : 0,
        payeeName: this.payeeForm.get('beneficiaryName')?.value,
        payeeAccountNumber: this.payeeForm.get('beneficiaryAccountNumber')?.value,
        userId:this.userId
      };

      if (this.editMode && this.selectedPayeeId !== null) {
        this.payeeService.updatePayee(payee).subscribe(
          () => {
            this.loadPayees();
            this.resetForm();
          },
          (error) => {
            window.alert('Error updating beneficiary: ' + error.message);
          }
        );
      } else {
        this.payeeService.createPayee(payee).subscribe(
          (createdPayee: Payee) => {
            this.loadPayees();
            this.resetForm();
          },
          (error) => {
            window.alert('Error creating beneficiary: ' + error.message);
          }
        );
      }
    }
  }

  onDelete(payeeId: number): void {
    this.payeeService.deletePayee(payeeId).subscribe(
      () => {
        this.loadPayees();
      },
      (error) => {
        window.alert('Error deleting beneficiary: ' + error.message);
      }
    )
  }

  onEdit(payee: Payee): void {
    this.editMode = true;
    this.selectedPayeeId = payee.payeeId;
    this.payeeForm.patchValue({
      beneficiaryName: payee.payeeName,
      beneficiaryAccountNumber: payee.payeeAccountNumber,
      reEnterAccountNumber: payee.payeeAccountNumber // to match the re-enter account number field
    });
  }

  resetForm(): void {
    this.payeeForm.reset();
    this.editMode = false;
    this.selectedPayeeId = null;
  }
}
