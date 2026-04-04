import { Route } from '@angular/router';
import {
  userAuthenticationGuard,
  userConfirmationGuard,
} from '@scm/auth/utils';

export const appRoutes: Route[] = [
  {
    path: 'auth',
    loadChildren: () => import('@scm/auth/features').then((i) => i.routes),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard',
  },
  {
    path: '',
    loadComponent: () =>
      import('./features/shell/shell.component').then((i) => i.ShellComponent),
    canActivate: [userAuthenticationGuard, userConfirmationGuard],
    canActivateChild: [userAuthenticationGuard, userConfirmationGuard],
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('@scm/containers/features').then(
            (m) => m.ContainersOverviewComponent,
          ),
      },
      {
        path: 'containers',
        loadChildren: () =>
          import('@scm/containers/features').then((i) => i.routes),
      },
      {
        path: 'users',
        loadChildren: () => import('@scm/users/features').then((i) => i.routes),
      },
    ],
  },
];
