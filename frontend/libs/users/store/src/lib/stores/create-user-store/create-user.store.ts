import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { withCreateUserState } from './create-user.state';
import { CreateUserRequest as LocalCreateUserRequest } from '../../models';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { generatePassword } from '../../utils';
import { firstValueFrom } from 'rxjs';
import { inject } from '@angular/core';
import { UsersService } from '@scm/users/data';

export const CreateUserStore = signalStore(
  withCreateUserState(),
  withMethods((store, usersService = inject(UsersService)) => {
    const createUser = async (request: LocalCreateUserRequest) => {
      try {
        patchState(store, { requestStatus: 'pending' }, clearError());
        const password = generatePassword();

        const response = await firstValueFrom(
          usersService.createUser({
            username: request.username,
            password,
          }),
        );

        patchState(store, {
          requestStatus: 'fulfilled',
          generatedPassword: password,
          createdUserId: response.userId,
        });
      } catch (error) {
        patchState(store, { requestStatus: 'idle' }, setError(error));
      }
    };

    return { createUser };
  }),
);
