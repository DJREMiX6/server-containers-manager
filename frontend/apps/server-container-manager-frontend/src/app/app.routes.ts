import { Route } from '@angular/router';
import { ShellComponent } from './features/shell/shell.component';

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
    component: ShellComponent,
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
