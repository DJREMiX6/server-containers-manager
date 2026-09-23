import { DatePipe, NgClass } from '@angular/common';
import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Tag } from 'primeng/tag';
import { ButtonGroup } from 'primeng/buttongroup';
import { Button } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { User, UsersListStore, provideUsersListStore } from '@scm/users/store';
import { CreateUserComponent } from '../create-user/create-user';
import { DeleteUser } from '../delete-user/delete-user';
import { ResetUserPassword } from '../reset-user-password/reset-user-password';
import { UserNamespaceAssignment } from '../user-namespace-assignment/user-namespace-assignment';

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
    DeleteUser,
    ResetUserPassword,
    UserNamespaceAssignment,
  ],
  providers: [provideUsersListStore()],
  templateUrl: './users-list.html',
})
export class UsersList implements OnInit {
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
  protected readonly isDeleteUserModalShown = signal<boolean>(false);
  protected readonly isResetUserPasswordModalShown = signal<boolean>(false);
  protected readonly isUserNamespacesAssignmentModalShown =
    signal<boolean>(false);
  protected readonly userToDelete = signal<User | null>(null);
  protected readonly userToResetPassword = signal<User | null>(null);
  protected readonly userToAssignNamespaces = signal<User | null>(null);

  ngOnInit(): void {
    this.usersListStore.ensureLoaded();
  }

  protected onCreateUserBtnClick() {
    this.isCreateUserModalShown.set(true);
  }

  protected onCreateUserOperationCanceled(): void {
    this.isCreateUserModalShown.set(false);
  }

  protected onCreateUserOperationCompleted(): void {
    this.isCreateUserModalShown.set(false);
    this.usersListStore.refresh();
  }

  protected onDeleteUserBtnClick(user: User): void {
    this.userToDelete.set(user);
    this.isDeleteUserModalShown.set(true);
  }

  protected onDeleteUserOperationCanceled(): void {
    this.isDeleteUserModalShown.set(false);
    this.userToDelete.set(null);
  }

  protected onDeleteUserOperationCompleted(): void {
    this.isDeleteUserModalShown.set(false);
    this.userToDelete.set(null);
    this.usersListStore.refresh();
  }

  protected onResetUserPasswordBtnClick(user: User): void {
    this.userToResetPassword.set(user);
    this.isResetUserPasswordModalShown.set(true);
  }

  protected onResetUserPasswordOperationCanceled(): void {
    this.isResetUserPasswordModalShown.set(false);
    this.userToResetPassword.set(null);
  }

  protected onResetUserPasswordOperationCompleted(): void {
    this.isResetUserPasswordModalShown.set(false);
    this.userToResetPassword.set(null);
    this.usersListStore.refresh();
  }

  protected onUserNamespacesAssignmentBtnClick(user: User): void {
    this.userToAssignNamespaces.set(user);
    this.isUserNamespacesAssignmentModalShown.set(true);
  }

  protected onUserNamespacesAssignmentOperationCanceled(): void {
    this.isUserNamespacesAssignmentModalShown.set(false);
    this.userToAssignNamespaces.set(null);
  }

  protected onUserNamespacesAssignmentOperationCompleted(): void {
    this.isUserNamespacesAssignmentModalShown.set(false);
    this.userToAssignNamespaces.set(null);
    this.usersListStore.refresh();
  }
}
