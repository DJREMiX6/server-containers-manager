import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Tag } from 'primeng/tag';
import { ButtonGroup } from 'primeng/buttongroup';
import { Button } from 'primeng/button';

type Namespace = {
  id: string;
  name: string;
};

type User = {
  id: string;
  username: string;
  isConfirmed: boolean;
  lastAccess: Date;
  role: 'Member' | 'Admin';
  namespaces: Namespace[];
};

@Component({
  selector: 'lib-users-features',
  imports: [TableModule, Tag, Button, ButtonGroup, DatePipe],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList {
  protected readonly users: User[] = [
    {
      id: '1',
      username: 'Admin',
      role: 'Admin',
      isConfirmed: true,
      lastAccess: new Date(),
      namespaces: [],
    },
    {
      id: '2',
      username: 'Stryn',
      role: 'Member',
      isConfirmed: true,
      lastAccess: new Date(2026, 2, 20, 12, 33),
      namespaces: [
        {
          id: '1',
          name: 'Games',
        },
      ],
    },
    {
      id: '3',
      username: 'Takatalvi',
      role: 'Member',
      isConfirmed: false,
      lastAccess: new Date(2026, 2, 10, 13, 3),
      namespaces: [
        {
          id: '1',
          name: 'Games',
        },
        {
          id: '2',
          name: 'Apps',
        },
      ],
    },
    {
      id: '4',
      username: 'Baride',
      role: 'Member',
      isConfirmed: true,
      lastAccess: new Date(2026, 1, 30, 8, 15),
      namespaces: [
        {
          id: '1',
          name: 'Games',
        },
        {
          id: '2',
          name: 'Apps',
        },
        {
          id: '3',
          name: 'System',
        },
      ],
    },
  ];
}
