import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  output,
  signal,
} from '@angular/core';
import { PickListModule } from 'primeng/picklist';
import { Button } from 'primeng/button';

type User = { id: string; username: string };

@Component({
  selector: 'lib-namespace-user-assignment',
  imports: [PickListModule, Button],
  templateUrl: './namespace-user-assignment.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceUserAssignmentFeature {
  public readonly selectedNamespaceId = input.required<undefined | string>();

  public readonly operationCanceled = output<void>();
  public readonly operationCompleted = output<void>();

  protected readonly sourceUsers = signal<User[]>([
    {
      id: '1',
      username: 'testUser1',
    },
    {
      id: '2',
      username: 'testUser2',
    },
    {
      id: '3',
      username: 'testUser3',
    },
    {
      id: '4',
      username: 'testUser4',
    },
    {
      id: '5',
      username: 'testUser5',
    },
  ]);
  protected readonly targetUsers = signal<User[]>([]);

  protected shouldShowSourceFilter() {
    return this.sourceUsers().length > 3;
  }

  protected shouldShowTargetFilter() {
    return this.targetUsers().length > 3;
  }

  protected onConfirmBtnClick(): void {
    this.operationCompleted.emit();
  }

  protected onCancelBtnClick(): void {
    this.operationCanceled.emit();
  }
}
