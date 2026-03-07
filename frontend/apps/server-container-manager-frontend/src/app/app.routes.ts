import { Route } from '@angular/router';
import { ShellComponent } from './features/shell/shell.component';

export const appRoutes: Route[] = [
  {
    path: 'auth',
    loadChildren: () => import('@scm/auth/features').then((m) => m.routes),
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
        loadChildren: () =>
          import('@scm/containers/features').then((m) => m.routes),
      },
      {
        path: 'containers',
        loadChildren: () =>
          import('@scm/containers/features').then((m) => m.routes),
      },
    ],
  },
];
