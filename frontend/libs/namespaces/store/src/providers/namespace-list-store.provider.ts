import { Provider } from '@angular/core';
import { NamespaceListStore } from '../stores/namespace-list/namespace-list.store';
import { NamespacesService } from '@scm/namespaces/data';

export function provideNamespaceListStore(): Provider[] {
  return [NamespacesService, NamespaceListStore];
}
