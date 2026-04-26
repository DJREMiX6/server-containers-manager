import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { withCreateUserState } from './create-user.state';
import { CreateUserRequest as LocalCreateUserRequest } from '../../models';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { generatePassword } from '../../utils';
import { firstValueFrom } from 'rxjs';
import { inject } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

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

    const checkUsernameAvailability = async (
      username: string,
    ): Promise<{ isAvailable: boolean }> => {
      try {
        patchState(store, clearError());

        await firstValueFrom(
          usersService.checkUsernameAvailability({ username }),
        );

        return { isAvailable: true };
      } catch (error) {
        if (
          error instanceof HttpErrorResponse &&
          error.status == HttpStatusCode.Conflict
        ) {
          return { isAvailable: false };
        }

        patchState(store, setError(error));
        throw error;
      }
    };

    return { createUser, checkUsernameAvailability };
  }),
);
