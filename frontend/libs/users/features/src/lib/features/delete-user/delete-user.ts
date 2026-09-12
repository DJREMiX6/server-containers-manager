import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import {
  form,
  required,
  validate,
  FormRoot,
  FormField,
} from '@angular/forms/signals';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import {
  DeleteUserStore,
  provideDeleteUserStore,
  User,
} from '@scm/users/store';
import { MessageService } from 'primeng/api';

export type DeleteUserFormModel = {
  username: string;
};

@Component({
  selector: 'lib-delete-user',
  imports: [FormRoot, FormField, InputText, Button, CommonModule],
  providers: [provideDeleteUserStore()],
  templateUrl: './delete-user.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteUser {
  protected readonly deleteUserStore = inject(DeleteUserStore);
  private readonly toastService = inject(MessageService);

  public readonly user = input.required<User | null>();

  public readonly operationCompleted = output<void>();
  public readonly operationCanceled = output<void>();

  private readonly deleteUserFormModel = signal<DeleteUserFormModel>({
    username: '',
  });

  private readonly onUserChange = effect(() => this.reset());

  private readonly onUserDeleteSuccessful = effect(() => {
    if (this.deleteUserStore.deleteUserStatus() !== 'completed') return;

    this.toastService.add({
      summary: 'User Deleted Successfully',
      detail: `The user ${this.user()?.username} was deleted successfully`,
      severity: 'success',
    });

    this.operationCompleted.emit();
  });

  private readonly onUserDeleteError = effect(() => {
    const error = this.deleteUserStore.error();
    if (!error) return;

    this.toastService.add({
      summary: 'User Deletion Error',
      detail: 'An unexpected error has ocurred',
      severity: 'danger',
    });
  });

  protected deleteUserForm = form(
    this.deleteUserFormModel,
    (schema) => {
      required(schema.username, { message: 'The Username is required' });
      validate(schema.username, ({ value }) => {
        const userToDelete = this.user();
        if (userToDelete === null)
          return {
            kind: 'UserNull',
            message: 'User to delete is null',
          };

        if (value() !== userToDelete.username)
          return {
            kind: 'UsernamesNotMatch',
            message: 'Username is not equal',
          };

        return null;
      });
    },
    {
      submission: {
        action: async () => {
          const userId = this.user()?.id;
          if (!userId) throw new Error('User is null or undefined');

          await this.deleteUserStore.deleteUser(userId);
        },
      },
    },
  );

  protected cancelOperation(): void {
    this.operationCanceled.emit();
    this.reset();
  }

  private reset(): void {
    this.deleteUserFormModel.set({ username: '' });
    this.deleteUserForm().reset();
    this.deleteUserStore.reset();
  }
}
