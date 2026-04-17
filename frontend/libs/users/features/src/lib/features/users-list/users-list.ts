import { DatePipe, NgClass } from '@angular/common';
import { Component, effect, inject, OnDestroy, OnInit } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Tag } from 'primeng/tag';
import { ButtonGroup } from 'primeng/buttongroup';
import { Button } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { UsersListStore, provideUsersListStore } from '@scm/users/store';

@Component({
  selector: 'lib-users-list-features',
  imports: [TableModule, Tag, Button, ButtonGroup, DatePipe, NgClass],
  providers: [provideUsersListStore()],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList implements OnInit, OnDestroy {
  private readonly toastService = inject(MessageService);
  protected readonly usersListStore = inject(UsersListStore);

  private readonly onUsersListStoreError = effect(() => {
    const error = this.usersListStore.error();
    if (!error) return;

    this.toastService.add({
      summary: error.title,
      detail: error.summary,
      severity: error.severity,
    });
  });

  ngOnInit(): void {
    this.usersListStore.ensureLoaded();
  }

  ngOnDestroy(): void {
    this.onUsersListStoreError.destroy();
  }
}
