import { Provider } from '@angular/core';
import { NamespacesService } from '@scm/namespaces/data';
import { UsersService } from '@scm/users/data';
import { NamespaceAssignUsersStore } from '../stores';

export function provideNamespaceAssignUserStore(): Provider[] {
  return [UsersService, NamespacesService, NamespaceAssignUsersStore];
}
