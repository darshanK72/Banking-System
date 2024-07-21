import { Address } from './address.model';

export interface Account {
  accountId?: number;
  accountNumber?: string;
  title: string;
  firstName: string;
  middleName?: string;
  lastName: string;
  fathersName: string;
  mobileNumber: string;
  emailId?: string;
  aadharCardNumber: string;
  dateOfBirth: Date;
  residentialAddress: Address;
  permanentAddress: Address;
  occupationType: string;
  sourceOfIncome: string;
  grossAnnualIncome: number;
  wantDebitCard: boolean;
  optForNetBanking: boolean;
  transactionPassword?:string;
  isApproved?: boolean;
  balance?: number;
  userId?: number;
  accountType: string;
}
