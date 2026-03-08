import { Route } from '@angular/router';
import { ContainersListComponent } from './containers-list/containers-list.component';

export const routes: Route[] = [
  {
    path: '',
    children: [
      {
        path: '',
        component: ContainersListComponent,
      },
    ],
  },
];