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
import { MessageService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';
import { CreateNamespaceFormModel, initialCreateNamespaceFormState } from '../../models';
import {
  provideCreateNamespaceStore,
  CreateNamespaceStore,
  CreateNamespaceRequest,
} from '@scm/namespaces/store';

@Component({
  selector: 'lib-create-namespace',
  imports: [
    FormRoot,
    FormField,
    FloatLabel,
    IconField,
    InputIcon,
    Message,
    Button,
    InputText,
    TooltipModule,
  ],
  providers: [provideCreateNamespaceStore()],
  templateUrl: './create-namespace.html',
  styleUrl: './create-namespace.css',
})
export class CreateNamespaceComponent {
  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<{ namespaceId: string }>();

  private readonly toastService = inject(MessageService);
  protected readonly createNamespaceStore = inject(CreateNamespaceStore);

  private readonly onCreateNamespaceSuccessful = effect(() => {
    if (this.createNamespaceStore.requestStatus() !== 'fulfilled') return;

    this.toastService.add({
      summary: 'User creation success',
      detail: `User ${this.formState().name} was created successfully`,
      severity: 'success',
    });
  });

  private readonly onCreateNamespaceError = effect(() => {
    const error = this.createNamespaceStore.error();
    if (!error || this.createNamespaceStore.requestStatus() !== 'idle') return;

    this.toastService.add({
      summary: error.title,
      detail: error.summary,
      severity: 'error',
    });
  });

  private readonly formState = signal<CreateNamespaceFormModel>({
    ...initialCreateNamespaceFormState,
  });

  protected readonly createNamespaceForm = form(
    this.formState,
    (schema) => {
      required(schema.name, { message: 'Name is required.' });
      minLength(schema.name, 3, {
        message: 'Name must be at least 3 characters long.',
      });
      debounce(schema.name, 300);
      validateAsync(schema.name, {
        params: ({ value }) => value(),
        factory: (params) =>
          resource({
            params,
            loader: async ({ params }) =>
              await this.createNamespaceStore.checkNameAvailability(params),
          }),
        onSuccess: ({ isAvailable }) => {
          if (isAvailable) return undefined;

          return {
            kind: 'NameAlreadyTaken',
            message: 'Name is already in use.',
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
          const request: CreateNamespaceRequest = {
            username: form.name().value(),
          };
          await this.createNamespaceStore.createNamespace(request);
        },
      },
    },
  );

  public reset(): void {
    this.formState.set({
      name: '',
    });
    this.createNamespaceForm().reset();
  }

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
  }

  protected onCloseBtnClick(): void {
    const createdNamespaceId = this.createNamespaceStore.createdNamespaceId();
    if (!createdNamespaceId) throw new Error('Created Namespace Id is null or undefined');

    this.operationCompleted.emit({
      namespaceId: createdNamespaceId,
    });
  }
}
