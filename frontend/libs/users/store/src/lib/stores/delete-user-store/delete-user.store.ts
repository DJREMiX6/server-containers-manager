import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { withDeleteUserState } from './delete-user-state';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { firstValueFrom } from 'rxjs';
import { inject } from '@angular/core';
import { UsersService } from '@scm/users/data';

export const DeleteUserStore = signalStore(
  withDeleteUserState(),
  withMethods((store, usersService = inject(UsersService)) => ({
    deleteUser: async (userId: string) => {
      try {
        patchState(store, { deleteUserStatus: 'pending' }, clearError());

        await firstValueFrom(usersService.deleteUser({ userId }));

        patchState(store, { deleteUserStatus: 'completed' });
      } catch (error) {
        patchState(store, { deleteUserStatus: 'error' }, setError(error));
      }
    },
    reset: () => {
      patchState(store, { deleteUserStatus: 'idle' }, clearError());
    },
  })),
);
