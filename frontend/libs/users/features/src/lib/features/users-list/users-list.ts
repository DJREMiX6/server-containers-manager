import { DatePipe, NgClass } from '@angular/common';
import {
  Component,
  effect,
  inject,
  OnDestroy,
  OnInit,
  signal,
  viewChild,
} from '@angular/core';
import { TableModule } from 'primeng/table';
import { Tag } from 'primeng/tag';
import { ButtonGroup } from 'primeng/buttongroup';
import { Button } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { UsersListStore, provideUsersListStore } from '@scm/users/store';
import { CreateUserComponent } from '../create-user/create-user';

@Component({
  selector: 'lib-users-list',
  imports: [
    TableModule,
    Tag,
    Button,
    ButtonGroup,
    TooltipModule,
    DatePipe,
    NgClass,
    Dialog,
    CreateUserComponent,
  ],
  providers: [provideUsersListStore()],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList implements OnInit, OnDestroy {
  private readonly createUserFeature =
    viewChild<CreateUserComponent>('CreateUserFeature');

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

  protected readonly isCreateUserModalShown = signal<boolean>(false);

  ngOnInit(): void {
    this.usersListStore.ensureLoaded();
  }

  ngOnDestroy(): void {
    this.onUsersListStoreError.destroy();
  }

  protected onCreateUserBtnClick() {
    this.isCreateUserModalShown.set(true);
  }

  protected onCreateUserModalHide(): void {
    this.createUserFeature()?.reset();
  }

  protected onCreateUserOperationCanceled(): void {
    this.isCreateUserModalShown.set(false);
  }

  protected onCreateUserOperationCompleted(): void {
    this.isCreateUserModalShown.set(false);
    this.usersListStore.refresh();
  }
}
