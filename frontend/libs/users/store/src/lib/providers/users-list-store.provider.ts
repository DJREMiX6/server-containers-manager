import { Provider } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { UsersListStore } from '../stores';

export function provideUsersListStore(): Provider[] {
  return [UsersService, UsersListStore];
}
