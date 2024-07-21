import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'src/app/Services/account.service';

@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.css']
})
export class UserHomeComponent implements OnInit{

  account!:any;
  isAccountApproved!:boolean;

  constructor(private accountService:AccountService,private route:ActivatedRoute){}

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
      this.accountService.account$.subscribe(ac => {
        this.account = ac;
        if(this.account?.isApproved){
          this.isAccountApproved = true;
        }else{
          this.isAccountApproved = false;
        }
      })
    })
  }
}
