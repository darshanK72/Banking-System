import { Transaction } from "./transaction.model";

export interface TransactionResponse{
    success:boolean;
    message:string;
    transaction:Transaction;
}