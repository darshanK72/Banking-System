import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterNetbankingComponent } from './register-netbanking.component';

describe('RegisterNetbankingComponent', () => {
  let component: RegisterNetbankingComponent;
  let fixture: ComponentFixture<RegisterNetbankingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RegisterNetbankingComponent]
    });
    fixture = TestBed.createComponent(RegisterNetbankingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
