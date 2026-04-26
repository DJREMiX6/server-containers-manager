import {
  Component,
  effect,
  inject,
  output,
  resource,
  signal,
} from '@angular/core';
import {
  FormRoot,
  FormField,
  form,
  required,
  minLength,
  validateAsync,
  debounce,
} from '@angular/forms/signals';
import { FloatLabel } from 'primeng/floatlabel';
import { IconField } from 'primeng/iconfield';
import { InputIcon } from 'primeng/inputicon';
import { Message } from 'primeng/message';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { StepperModule } from 'primeng/stepper';
import { MessageService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';
import { CreateUserFormModel, initialCreateUserFormState } from '../../models';
import {
  provideCreateUserStore,
  CreateUserStore,
  CreateUserRequest,
} from '@scm/users/store';

@Component({
  selector: 'lib-create-user',
  imports: [
    FormRoot,
    FormField,
    FloatLabel,
    IconField,
    InputIcon,
    Message,
    Button,
    InputText,
    InputGroup,
    InputGroupAddon,
    StepperModule,
    TooltipModule,
  ],
  providers: [provideCreateUserStore()],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css',
})
export class CreateUserComponent {
  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<{ userId: string }>();

  private readonly toastService = inject(MessageService);
  protected readonly createUserStore = inject(CreateUserStore);

  private readonly onCreateUserSuccessful = effect(() => {
    if (this.createUserStore.requestStatus() !== 'fulfilled') return;

    this.toastService.add({
      summary: 'User creation success',
      detail: `User ${this.formState().username} was created successfully`,
      severity: 'success',
    });
    this.showCopyPasswordStep();
  });

  private readonly onCreateUserError = effect(() => {
    const error = this.createUserStore.error();
    if (!error || this.createUserStore.requestStatus() !== 'idle') return;

    this.toastService.add({
      summary: error.title,
      detail: error.summary,
      severity: 'error',
    });
  });

  private readonly formState = signal<CreateUserFormModel>({
    ...initialCreateUserFormState,
  });

  protected readonly createUserForm = form(
    this.formState,
    (schema) => {
      required(schema.username, { message: 'Username is required.' });
      minLength(schema.username, 3, {
        message: 'Username must be at least 3 characters long.',
      });
      debounce(schema.username, 300);
      validateAsync(schema.username, {
        params: ({ value }) => value(),
        factory: (params) =>
          resource({
            params,
            loader: async ({ params }) =>
              await this.createUserStore.checkUsernameAvailability(params),
          }),
        onSuccess: ({ isAvailable }) => {
          if (isAvailable) return undefined;

          return {
            kind: 'UsernameAlreadyTaken',
            message: 'Username is already in use.',
          };
        },
        onError: (error) => {
          console.error(error);
          this.toastService.add({
            summary: 'Unexpected error',
            detail: 'An unexpected error has ocurred',
          });
          return {
            kind: 'Unexpected error',
            message:
              'An unexpected error has ocurred, colud not validate the username.',
          };
        },
      });
    },
    {
      submission: {
        action: async (form) => {
          const request: CreateUserRequest = {
            username: form.username().value(),
          };
          await this.createUserStore.createUser(request);
        },
      },
    },
  );

  protected readonly userCopiedPassword = signal(false);
  protected readonly step = signal(1);

  public reset(): void {
    this.showFormStep();
    this.userCopiedPassword.set(false);
    this.formState.set({
      username: '',
    });
    this.createUserForm().reset();
  }

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
  }

  protected onCloseBtnClick(): void {
    const createdUserId = this.createUserStore.createdUserId();
    if (!createdUserId) throw new Error('Created UserId is null or undefined');

    this.operationCompleted.emit({
      userId: createdUserId,
    });
  }

  protected async onCopyPasswordBtnClick(): Promise<void> {
    const password = this.createUserStore.generatedPassword();
    if (!password) throw new Error('Password is null or undefined.');

    await globalThis.navigator.clipboard.writeText(password);

    this.userCopiedPassword.set(true);
  }

  private showFormStep(): void {
    this.step.set(1);
  }

  private showCopyPasswordStep(): void {
    this.step.set(2);
  }
}
