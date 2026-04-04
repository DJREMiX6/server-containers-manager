import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { MessageService } from 'primeng/api';
import { AuthStore } from '@scm/auth/store';
import { ChangePasswordFormModel } from '../../models';
import { form, FormField, required, submit, validate } from '@angular/forms/signals';

@Component({
  selector: 'lib-change-password',
  imports: [
    IconFieldModule,
    InputIconModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
    FloatLabelModule,
    FormField,
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent {
  private readonly authStore = inject(AuthStore);
  private readonly toastService = inject(MessageService);
  private readonly router = inject(Router);

  protected isBuisy = signal<boolean>(false);

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
      /* try {
        const loginRequest: LoginRequestModel = {
          username: form().value().username,
          password: form().value().password,
        };

        await this.authStore.login(loginRequest);
        this.toastService.add({
          summary: 'Login successful',
          severity: 'success',
        });

        this.router.navigate(['']);
      } catch (error) {
        console.error(error);

        this.toastService.add({
          summary: 'Invalid login credentials',
          severity: 'error',
        });
      } */
    }).finally(() => this.isBuisy.set(false));
  }
}
