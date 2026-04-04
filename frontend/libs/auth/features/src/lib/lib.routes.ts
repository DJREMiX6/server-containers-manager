import { Route } from '@angular/router';

export const routes: Route[] = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features').then((i) => i.LoginFeatureComponent),
  },
  {
    path: 'user/change-password',
    loadComponent: () =>
      import('./features').then((i) => i.ChangePasswordComponent),
  },
];
