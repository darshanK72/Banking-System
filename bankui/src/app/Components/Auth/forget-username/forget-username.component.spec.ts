import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgetUsernameComponent } from './forget-username.component';

describe('ForgetUsernameComponent', () => {
  let component: ForgetUsernameComponent;
  let fixture: ComponentFixture<ForgetUsernameComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForgetUsernameComponent]
    });
    fixture = TestBed.createComponent(ForgetUsernameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
