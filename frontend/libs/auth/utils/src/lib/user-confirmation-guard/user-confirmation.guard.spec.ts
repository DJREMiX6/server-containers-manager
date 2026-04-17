import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { userConfirmationGuard } from './user-confirmation.guard';

describe('userConfirmationGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => userConfirmationGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
