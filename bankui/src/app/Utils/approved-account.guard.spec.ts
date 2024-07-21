import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { approvedAccountGuard } from './approved-account.guard';

describe('approvedAccountGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => approvedAccountGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
