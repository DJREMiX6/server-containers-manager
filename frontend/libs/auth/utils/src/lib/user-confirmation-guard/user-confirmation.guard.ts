import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '@scm/auth/store';

export const userConfirmationGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  const user = authStore.user();
  if(!user)
    throw new Error("User null or undefined.");

  if(!user.isConfirmed)
    return router.createUrlTree(["auth", "user", "change-password"]);

  return true;
};
