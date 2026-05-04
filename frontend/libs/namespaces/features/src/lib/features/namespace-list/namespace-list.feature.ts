import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
} from '@angular/core';
import { TableModule } from 'primeng/table';
import {
  provideNamespaceListStore,
  NamespaceListStore,
} from '@scm/namespaces/store';

@Component({
  selector: 'lib-namespace-list-feature',
  imports: [TableModule],
  providers: [provideNamespaceListStore()],
  templateUrl: './namespace-list.feature.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NamespaceListFeature implements OnInit {
  protected readonly namespaceListStore = inject(NamespaceListStore);

  ngOnInit(): void {
    this.namespaceListStore.ensureLoaded();
  }
}
