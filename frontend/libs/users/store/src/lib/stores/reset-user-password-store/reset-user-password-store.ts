import { patchState, signalStore, withMethods } from '@ngrx/signals';
import {
  initialState,
  withResetUserPasswordState,
} from './reset-user-password-state';
import { inject } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { generatePassword } from '../../utils';
import { firstValueFrom } from 'rxjs';

export const ResetUserPasswordStore = signalStore(
  withResetUserPasswordState(),
  withMethods((store, usersService = inject(UsersService)) => ({
    resetPassword: async (userId: string) => {
      try {
        const generatedPassword = generatePassword();
        patchState(store, {
          resetStatus: 'pending',
          userIdToReset: userId,
        });

        await firstValueFrom(
          usersService.resetUserPassword({
            userId,
            password: generatedPassword,
          }),
        );

        patchState(store, {
          resetStatus: 'successful',
          generatedPassword,
        });
      } catch (error) {
        patchState(
          store,
          setError(error),
          {
            resetStatus: 'error',
            generatedPassword: null,
          },
          clearError(),
        );
      }
    },
    reset: () => {
      patchState(store, clearError(), {
        ...initialState,
      });
    },
  })),
);
