export interface Transaction {
  transactionId: number;
  description?: string;
  amount: number;
  transactionDate: Date;
  fromAccountId: number;
  fromAccountNumber?: string;
  toAccountId: number;
  toAccountNumber?: string;
  transactionType: string;
  status?: string;
  remark?: string;
  maturityInstructions?: string;
}