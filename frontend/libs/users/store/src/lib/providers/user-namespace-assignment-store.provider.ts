import { Provider } from '@angular/core';
import { UserNamespaceAssignmentStore } from '../stores';
import { NamespacesService } from '@scm/namespaces/data';
import { UsersService } from '@scm/users/data';

export function provideUserNamespaceAssignmentStore(): Provider[] {
  return [UserNamespaceAssignmentStore, NamespacesService, UsersService];
}
