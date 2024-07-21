import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { accountGuardGuard } from './account.guard';

describe('accountGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => accountGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
