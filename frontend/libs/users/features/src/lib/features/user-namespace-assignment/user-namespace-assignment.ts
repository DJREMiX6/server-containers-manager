import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import {
  provideUserNamespaceAssignmentStore,
  User,
  UserNamespaceAssignmentStore,
} from '@scm/users/store';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { PickList } from 'primeng/picklist';

@Component({
  selector: 'lib-user-namespace-assignment',
  imports: [Button, PickList],
  providers: [provideUserNamespaceAssignmentStore()],
  templateUrl: './user-namespace-assignment.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserNamespaceAssignment {
  private readonly toastService = inject(MessageService);
  protected readonly userNamespacesAssignmentStore = inject(
    UserNamespaceAssignmentStore,
  );

  public readonly user = input.required<User | null>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  private readonly onUserChange = effect(async () => {
    const user = this.user();
    if (!user) return;

    await this.userNamespacesAssignmentStore.ensureLoaded(user);
  });

  private readonly onError = effect(() => {
    const error = this.userNamespacesAssignmentStore.error();
    if (!error) return;

    if (this.userNamespacesAssignmentStore.updateStatus() === 'error')
      this.toastService.add({
        severity: 'error',
        summary: 'Updating associated users failed',
        detail:
          'An error has ocurred updating the associated users, please retry.',
      });
    else if (
      this.userNamespacesAssignmentStore.namespacesLoadingStatus() ===
      'not-loaded'
    )
      this.toastService.add({
        severity: 'error',
        summary: 'Loading associated users failed',
        detail:
          'An error has ocurred loading the associated users for the current namespace, please retry.',
      });

    this.toastService.add({
      severity: 'error',
      summary: 'Unexpected Error',
      detail: 'An unexpected error has ocurred.',
    });
  });

  private readonly onOperationSuccess = effect(() => {
    if (this.userNamespacesAssignmentStore.updateStatus() !== 'changed') return;

    this.toastService.add({
      severity: 'success',
      summary: 'Namespace users updated',
    });
    this.operationCompleted.emit();
    this.userNamespacesAssignmentStore.reset();
  });

  protected readonly isLoading = computed(
    () =>
      this.userNamespacesAssignmentStore.namespacesLoadingStatus() ===
      'loading',
  );

  protected readonly isUpdating = computed(
    () => this.userNamespacesAssignmentStore.updateStatus() === 'pending',
  );

  protected readonly associatedNamespaces = computed(() => {
    return [...this.userNamespacesAssignmentStore.associatedNamespaces()];
  });

  protected readonly unassociatedNamespaces = computed(() => [
    ...this.userNamespacesAssignmentStore.unassociatedNamespaces(),
  ]);

  protected readonly picklistPt = {
    root: {
      class: 'h-full',
    },
    sourceControls: {
      hidden: true,
    },
    targetControls: {
      hidden: true,
    },
  };

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
  }

  protected async onConfirmBtnClick(): Promise<void> {
    await this.userNamespacesAssignmentStore.updateAssociatedNamespaces(
      this.associatedNamespaces(),
    );
  }

  protected shouldShowSourceFilter(): boolean {
    return (
      this.userNamespacesAssignmentStore.unassociatedNamespaces().length >= 6
    );
  }

  protected shouldShowTargetFilter(): boolean {
    return (
      this.userNamespacesAssignmentStore.associatedNamespaces().length >= 6
    );
  }
}
