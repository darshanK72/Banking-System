import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from 'src/app/Services/account.service';

@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.css']
})
export class UserHomeComponent implements OnInit{

  account!:any;

  constructor(private accountService:AccountService){}

  ngOnInit(): void {
      this.accountService.account$.subscribe(ac => {
        this.account = ac;
      })
  }
}
