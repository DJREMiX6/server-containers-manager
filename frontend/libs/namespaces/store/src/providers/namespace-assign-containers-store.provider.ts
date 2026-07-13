import { Provider } from '@angular/core';
import { NamespacesService } from '@scm/namespaces/data';
import { NamespaceAssignContainersStore } from '../stores';
import { ContainersService } from '@scm/containers/data';

export function provideNamespaceAssignContainerStore(): Provider[] {
  return [ContainersService, NamespacesService, NamespaceAssignContainersStore];
}
