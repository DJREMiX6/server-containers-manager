import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
} from '@angular/core';
import { PickListModule } from 'primeng/picklist';
import { Button } from 'primeng/button';
import {
  NamespaceAssignContainersStore,
  provideNamespaceAssignContainerStore,
} from '@scm/namespaces/store';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'lib-namespace-container-assignment',
  imports: [PickListModule, Button],
  providers: [provideNamespaceAssignContainerStore()],
  templateUrl: './namespace-container-assignment.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceContainerAssignmentFeature {
  private readonly toastService = inject(MessageService);
  protected readonly namespaceAssignContainerStore = inject(
    NamespaceAssignContainersStore,
  );

  public readonly selectedNamespaceId = input.required<string | undefined>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  private readonly onSelectedNamespaceIdChanges = effect(async () => {
    const selectedNamespaceId = this.selectedNamespaceId();
    if (!selectedNamespaceId) return;

    await this.namespaceAssignContainerStore.selectNamespace(selectedNamespaceId);
  });

  private readonly onError = effect(() => {
    const error = this.namespaceAssignContainerStore.error();
    if (!error) return;

    if (this.namespaceAssignContainerStore.associatedContainersUpdateStatus() === 'error')
      this.toastService.add({
        severity: 'error',
        summary: 'Updating associated users failed',
        detail:
          'An error has ocurred updating the associated users, please retry.',
      });
    else if (
      this.namespaceAssignContainerStore.namespaceContainersLoadingStatus() === 'error'
    )
      this.toastService.add({
        severity: 'error',
        summary: 'Loading associated users failed',
        detail:
          'An error has ocurred loading the associated users for the current namespace, please retry.',
      });
    else if (this.namespaceAssignContainerStore.containersLoadingStatus() === 'error')
      this.toastService.add({
        severity: 'error',
        summary: 'Loading users failed',
        detail: 'An error has ocurred loading the users, please retry.',
      });
  });

  private readonly onOperationSuccess = effect(() => {
    if (
      this.namespaceAssignContainerStore.associatedContainersUpdateStatus() !== 'changed'
    )
      return;

    this.toastService.add({
      severity: 'success',
      summary: 'Namespace users updated',
    });
    this.operationCompleted.emit();
  });

  protected readonly associatedContainers = computed(() => {
    return [...this.namespaceAssignContainerStore.associatedContainers()];
  });

  protected readonly unassociatedContainers = computed(() => [
    ...this.namespaceAssignContainerStore.unassociatedContainers(),
  ]);

  protected readonly isLoading = computed(
    () =>
      this.namespaceAssignContainerStore.namespaceContainersLoadingStatus() ===
        'loading' ||
      this.namespaceAssignContainerStore.namespaceContainersLoadingStatus() === 'loading',
  );

  protected readonly isUpdating = computed(
    () =>
      this.namespaceAssignContainerStore.associatedContainersUpdateStatus() === 'pending',
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
    return this.namespaceAssignContainerStore.unassociatedContainers().length >= 6;
  }

  protected shouldShowTargetFilter() {
    return this.namespaceAssignContainerStore.associatedContainers().length >= 6;
  }

  protected async onConfirmBtnClick(): Promise<void> {
    await this.namespaceAssignContainerStore.updateAssociatedContainers(
      this.associatedContainers(),
    );
  }

  protected onCancelBtnClick(): void {
    this.namespaceAssignContainerStore.resetAssociatedContainers();
    this.operationCanceled.emit();
  }
}
