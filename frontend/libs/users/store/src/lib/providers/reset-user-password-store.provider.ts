import { Provider } from '@angular/core';
import { ResetUserPasswordStore } from '../stores';
import { UsersService } from '@scm/users/data';

export function provideResetUserPasswordStore(): Provider[] {
  return [ResetUserPasswordStore, UsersService];
}
