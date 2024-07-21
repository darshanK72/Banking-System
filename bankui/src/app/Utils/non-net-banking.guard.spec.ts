import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { nonNetBankingGuard } from './non-net-banking.guard';

describe('nonNetBankingGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => nonNetBankingGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
