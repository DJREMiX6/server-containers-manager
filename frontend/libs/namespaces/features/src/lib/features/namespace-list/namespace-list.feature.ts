import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  OnInit,
  signal,
  viewChild,
} from '@angular/core';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';
import { ButtonGroup } from 'primeng/buttongroup';
import { Tooltip } from 'primeng/tooltip';
import { Dialog } from 'primeng/dialog';
import {
  provideNamespaceListStore,
  NamespaceListStore,
} from '@scm/namespaces/store';
import { MessageService } from 'primeng/api';
import { CreateNamespaceFeature } from '../create-namespace/create-namespace.feature';

@Component({
  selector: 'lib-namespace-list-feature',
  imports: [
    TableModule,
    Button,
    ButtonGroup,
    Tooltip,
    Dialog,
    CreateNamespaceFeature,
  ],
  providers: [provideNamespaceListStore()],
  templateUrl: './namespace-list.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceListFeature implements OnInit {
  private readonly createNamespaceFeature = viewChild<CreateNamespaceFeature>(
    'CreateNamespaceFeature',
  );

  private readonly toastService = inject(MessageService);
  protected readonly namespaceListStore = inject(NamespaceListStore);

  private readonly onNamespacesListStoreError = effect(() => {
    const error = this.namespaceListStore.error();
    if (!error) return;

    this.toastService.add({
      summary: error.title,
      detail: error.summary,
      severity: error.severity,
    });
  });

  protected readonly isCreateNamespaceModalShown = signal<boolean>(false);

  ngOnInit(): void {
    this.namespaceListStore.ensureLoaded();
  }

  protected onCreateNamespaceBtnClick() {
    this.isCreateNamespaceModalShown.set(true);
  }

  protected onCreateNamespaceModalHide(): void {
    this.createNamespaceFeature()?.reset();
  }

  protected onCreateNamespaceOperationCanceled(): void {
    this.isCreateNamespaceModalShown.set(false);
  }

  protected onCreateNamespaceOperationCompleted(): void {
    this.isCreateNamespaceModalShown.set(false);
    this.namespaceListStore.refresh();
  }
}
