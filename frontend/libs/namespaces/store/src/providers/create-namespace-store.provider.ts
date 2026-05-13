import { Provider } from '@angular/core';
import { CreateNamespaceStore } from '../stores';
import { NamespacesService } from '@scm/namespaces/data';

export function provideCreateNamespaceStore(): Provider[] {
  return [CreateNamespaceStore, NamespacesService];
}
