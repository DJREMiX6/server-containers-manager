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
  provideResetUserPasswordStore,
  ResetUserPasswordStore,
  User,
} from '@scm/users/store';
import { Button } from 'primeng/button';
import { CopyTextField } from '@scm/users/ui';
import { StepperModule } from 'primeng/stepper';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'lib-reset-user-password',
  imports: [CopyTextField, Button, StepperModule],
  providers: [provideResetUserPasswordStore()],
  templateUrl: './reset-user-password.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetUserPassword {
  private readonly toastService = inject(MessageService);
  protected readonly resetUserPasswordStore = inject(ResetUserPasswordStore);

  public readonly user = input.required<User | null>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  private readonly onPasswordResetSuccessfully = effect(() => {
    if (this.resetUserPasswordStore.resetStatus() !== 'successful') return;

    this.step.set(this.CopyTemporaryPasswordStepValue);

    this.toastService.add({
      summary: 'User password reset successful',
      detail: `The password of ${this.user()?.username} was successfully reset.`,
      severity: 'success',
    });
  });

  private readonly onResetUserPasswordStoreError = effect(() => {
    const error = this.resetUserPasswordStore.error();
    if (!error) return;

    this.toastService.add({
      summary: 'User password reset failed',
      detail: `An unexpected error has ocurred while resetting the ${this.user()?.username} password.`,
      severity: 'danger',
    });
  });

  protected readonly ConfirmPasswordResetStepValue = 1;
  protected readonly CopyTemporaryPasswordStepValue = 2;

  protected readonly passwordCopied = signal<boolean>(false);
  protected readonly step = signal<number>(this.ConfirmPasswordResetStepValue);

  protected onPasswordCopied(): void {
    this.passwordCopied.set(true);
  }

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
    this.reset();
  }

  protected async onResetPasswordBtnClick() {
    const user = this.user();
    if (!user) throw new Error('User is null or undefined.');

    await this.resetUserPasswordStore.resetPassword(user.id);
  }

  protected onConfirmBtnClick(): void {
    this.operationCompleted.emit();
    this.reset();
  }

  private reset(): void {
    this.passwordCopied.set(false);
    this.step.set(this.ConfirmPasswordResetStepValue);
    this.resetUserPasswordStore.reset();
  }
}
