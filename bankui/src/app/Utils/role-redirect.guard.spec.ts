import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { roleRedirectGuradGuard } from './role-redirect.guard';

describe('roleRedirectGuradGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => roleRedirectGuradGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
