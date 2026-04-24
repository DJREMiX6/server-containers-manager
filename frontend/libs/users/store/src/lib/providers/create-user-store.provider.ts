import { Provider } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { CreateUserStore } from '../stores';

export function provideCreateUserStore(): Provider[] {
  return [UsersService, CreateUserStore];
}
