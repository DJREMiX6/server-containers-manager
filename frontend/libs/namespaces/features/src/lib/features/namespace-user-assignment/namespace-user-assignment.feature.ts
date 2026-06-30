import {
  ChangeDetectionStrategy,
  Component,
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

@Component({
  selector: 'lib-namespace-user-assignment',
  imports: [PickListModule, Button],
  providers: [provideNamespaceAssignUserStore()],
  templateUrl: './namespace-user-assignment.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceUserAssignmentFeature {
  protected readonly namespaceAssignUserStore = inject(
    NamespaceAssignUsersStore,
  );

  public readonly selectedNamespaceId = input.required<undefined | string>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  private readonly onSelectedNamespaceIdChanges = effect(async () => {
    const selectedNamespaceId = this.selectedNamespaceId();
    if (!selectedNamespaceId) return;

    await this.namespaceAssignUserStore.selectNamespace(selectedNamespaceId);
  });

  protected readonly targetUsers = signal<User[]>([]);

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
    return this.namespaceAssignUserStore.unassignedUsers().length >= 6;
  }

  protected shouldShowTargetFilter() {
    return this.namespaceAssignUserStore.assignedUsers().length >= 6;
  }

  protected onConfirmBtnClick(): void {
    this.operationCompleted.emit();
  }

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
  }
}
