export interface NetBankingRequest {
    accountNumber: string;
    userId: number;
    userPassword: string;
    transactionPassword: string;
    otp: number;
}
