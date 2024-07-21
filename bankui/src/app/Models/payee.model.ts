import { Account } from "./account.model";

export interface Payee {
  payeeId: number;
  payeeName: string;
  payeeAccountNumber: string;
  payeeAccountId?: number;
  payeeAccount?: Account;
  userId?:number;
}