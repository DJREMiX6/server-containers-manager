import {
  ChangeDetectionStrategy,
  Component,
  effect,
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

export type DeleteUserFormModel = {
  username: string;
};

@Component({
  selector: 'lib-delete-user',
  imports: [FormRoot, FormField, InputText, Button, CommonModule],
  templateUrl: './delete-user.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeleteUser {
  public readonly username = input.required<string | null>();

  public readonly operationConfirmed = output<void>();
  public readonly operationCanceled = output<void>();

  private readonly deleteUserFormModel = signal<DeleteUserFormModel>({
    username: '',
  });

  protected readonly onUsernameChange = effect(() => {
    this.reset();
  });

  protected deleteUserForm = form(this.deleteUserFormModel, (schema) => {
    required(schema.username, { message: 'The Username is required' });
    validate(schema.username, ({ value }) => {
      const usernameToDelete = this.username();
      if (usernameToDelete === null || usernameToDelete == '')
        return {
          kind: 'UsernameNullOrEmpty',
          message: 'Username of user to delete is null or empty',
        };

      if (value() !== this.username())
        return {
          kind: 'UsernamesNotMatch',
          message: 'Username is not equal',
        };

      return null;
    });
  });

  protected cancelOperation(): void {
    this.operationCanceled.emit();
    this.reset();
  }

  private reset(): void {
    this.deleteUserFormModel.set({ username: '' });
    this.deleteUserForm().reset();
  }
}
