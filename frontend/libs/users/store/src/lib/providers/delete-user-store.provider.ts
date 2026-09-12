import { Provider } from '@angular/core';
import { DeleteUserStore } from '../stores';
import { UsersService } from '@scm/users/data';

export function provideDeleteUserStore(): Provider[] {
  return [DeleteUserStore, UsersService];
}
