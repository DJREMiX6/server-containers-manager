import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '@scm/auth/store';

export const userAuthenticationGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  const user = authStore.user();
  if(!user || !authStore.isAuthenticated)
    return router.createUrlTree(["auth", "login"]);

  return true;
};
