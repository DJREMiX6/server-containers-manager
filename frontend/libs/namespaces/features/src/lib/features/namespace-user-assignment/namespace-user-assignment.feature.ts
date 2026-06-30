import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { PickListModule } from 'primeng/picklist';
import { Button } from 'primeng/button';
import {
  NamespaceAssignUsersStore,
  provideNamespaceAssignUserStore,
} from '@scm/namespaces/store';
import { User } from '@scm/users/store';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'lib-namespace-user-assignment',
  imports: [PickListModule, Button],
  providers: [provideNamespaceAssignUserStore()],
  templateUrl: './namespace-user-assignment.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceUserAssignmentFeature {
  private readonly toastService = inject(MessageService);
  protected readonly namespaceAssignUserStore = inject(
    NamespaceAssignUsersStore,
  );

  public readonly selectedNamespaceId = input.required<string | undefined>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  private readonly onSelectedNamespaceIdChanges = effect(async () => {
    const selectedNamespaceId = this.selectedNamespaceId();
    if (!selectedNamespaceId) return;

    await this.namespaceAssignUserStore.selectNamespace(selectedNamespaceId);
  });

  private readonly onError = effect(() => {
    const error = this.namespaceAssignUserStore.error();
    if (!error) return;

    if (this.namespaceAssignUserStore.associatedUsersUpdateStatus() === 'error')
      this.toastService.add({
        severity: 'error',
        summary: 'Updating associated users failed',
        detail:
          'An error has ocurred updating the associated users, please retry.',
      });
    else if (
      this.namespaceAssignUserStore.namespaceUsersLoadingStatus() === 'error'
    )
      this.toastService.add({
        severity: 'error',
        summary: 'Loading associated users failed',
        detail:
          'An error has ocurred loading the associated users for the current namespace, please retry.',
      });
    else if (this.namespaceAssignUserStore.usersLoadingStatus() === 'error')
      this.toastService.add({
        severity: 'error',
        summary: 'Loading users failed',
        detail: 'An error has ocurred loading the users, please retry.',
      });
  });

  private readonly onOperationSuccess = effect(() => {
    if (
      this.namespaceAssignUserStore.associatedUsersUpdateStatus() !== 'changed'
    )
      return;

    this.toastService.add({
      severity: 'success',
      summary: 'Namespace users updated',
    });
    this.operationCompleted.emit();
  });

  protected readonly associatedUsers = computed(() => {
    return [...this.namespaceAssignUserStore.associatedUsers()];
  });

  protected readonly unassociatedUsers = computed(() => [
    ...this.namespaceAssignUserStore.unassociatedUsers(),
  ]);

  protected readonly isLoading = computed(
    () =>
      this.namespaceAssignUserStore.namespaceUsersLoadingStatus() ===
        'loading' ||
      this.namespaceAssignUserStore.namespaceUsersLoadingStatus() === 'loading',
  );

  protected readonly isUpdating = computed(
    () =>
      this.namespaceAssignUserStore.associatedUsersUpdateStatus() === 'pending',
  );

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

  protected shouldShowSourceFilter() {
    return this.namespaceAssignUserStore.unassociatedUsers().length >= 6;
  }

  protected shouldShowTargetFilter() {
    return this.namespaceAssignUserStore.associatedUsers().length >= 6;
  }

  protected async onConfirmBtnClick(): Promise<void> {
    await this.namespaceAssignUserStore.updateAssociatedUsers(
      this.associatedUsers(),
    );
  }

  protected onCancelBtnClick(): void {
    this.namespaceAssignUserStore.resetAssociatedUsers();
    this.operationCanceled.emit();
  }
}
