import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { netBankingEnabledGuard } from './net-banking-enabled.guard';

describe('netBankingEnabledGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => netBankingEnabledGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
