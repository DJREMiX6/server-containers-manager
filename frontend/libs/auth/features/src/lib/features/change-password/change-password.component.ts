import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { Message } from 'primeng/message';
import { MessageService } from 'primeng/api';
import { AuthStore, ChangePasswordRequestModel } from '@scm/auth/store';
import { ChangePasswordFormModel } from '../../models';
import {
  form,
  FormField,
  minLength,
  required,
  submit,
  validate,
  pattern,
} from '@angular/forms/signals';

@Component({
  selector: 'lib-change-password',
  imports: [
    IconFieldModule,
    InputIconModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
    FloatLabelModule,
    Message,
    FormField,
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent {
  private readonly authStore = inject(AuthStore);
  private readonly toastService = inject(MessageService);
  private readonly router = inject(Router);

  protected readonly isUserConfirmed =
    this.authStore.user()?.isConfirmed ?? false;

  protected readonly isBuisy = signal<boolean>(false);

  private changePasswordModel = signal<ChangePasswordFormModel>({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });

  protected changePasswordForm = form(this.changePasswordModel, (schema) => {
    required(schema.currentPassword, {
      message: 'Current password is required',
    });

    required(schema.newPassword, { message: 'New password is required' });

    minLength(schema.newPassword, 6, {
      message: `New password must be at least 6 characters long`,
    });

    pattern(schema.newPassword, /[a-z]/, {
      message: 'Must contain at least one lowercase letter',
    });

    pattern(schema.newPassword, /[A-Z]/, {
      message: 'Must contain at least one uppercase letter',
    });

    pattern(schema.newPassword, /\d/, {
      message: 'Must contain at least one number',
    });

    pattern(schema.newPassword, /[!@#$%^&*()_\-+=\][{};:'",.<>/?\\|`~]/, {
      message:
        'Must contain at least one special character: ! @ # $ % ^ & * ( ) _ - + = [ ] { } ; : \' " , . < > / ? \\ | ` ~',
    });

    validate(schema.newPassword, ({ value, valueOf }) => {
      const newPassword = value();
      const currentPassword = valueOf(schema.currentPassword);

      if (newPassword !== currentPassword) return null;

      return {
        kind: 'newPasswordMatch',
        message: 'Current password and New password must not be equal',
      };
    });

    required(schema.confirmPassword, {
      message: 'Confirm password is required',
    });
    validate(schema.confirmPassword, ({ value, valueOf }) => {
      const confirmPassword = value();
      const newPassword = valueOf(schema.newPassword);

      if (confirmPassword === newPassword) return null;

      return {
        kind: 'confirmPasswordMismatch',
        message: 'New password and Confirm password must be equal',
      };
    });
  });

  protected async onFormSubmit(event: Event) {
    event.preventDefault();

    if (this.isBuisy()) return;

    this.isBuisy.set(true);

    await submit(this.changePasswordForm, async (form) => {
      try {
        const changePasswordRequest: ChangePasswordRequestModel = {
          currentPassword: form.currentPassword().value(),
          newPassword: form.newPassword().value(),
        };

        await this.authStore.changePassword(changePasswordRequest);
        this.toastService.add({
          summary: 'Password change successful',
          severity: 'success',
        });

        this.router.navigate(['auth', 'login']);
      } catch (error) {
        console.error(error);

        this.toastService.add({
          summary: 'Invalid Password',
          severity: 'error',
        });
      }
    }).finally(() => this.isBuisy.set(false));
  }
}
