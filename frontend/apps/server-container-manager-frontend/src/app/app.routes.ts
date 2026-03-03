import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: 'auth',
    loadChildren: () => import('@scm/auth/features').then((m) => m.routes),
  },
];
