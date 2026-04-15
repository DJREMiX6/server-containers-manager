import {
  patchState,
  signalStore,
  withComputed,
  withMethods,
} from '@ngrx/signals';
import { withUsersListState } from './users-list.state';
import { computed, inject } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { firstValueFrom } from 'rxjs';
import { getUsersResponseMapper } from '../../mappers';
import { setEntities } from '@ngrx/signals/entities';

export const UsersListStore = signalStore(
  withUsersListState(),
  withComputed((store) => ({
    users: computed(() => store.entities()),
  })),
  withMethods((store, usersService = inject(UsersService)) => {
    const loadUsers = async () => {
      try {
        patchState(store, { loadingStatus: 'loading' }, clearError());

        const response = await firstValueFrom(usersService.getUsers());
        const users = getUsersResponseMapper(response);

        patchState(
          store,
          {
            loadingStatus: 'loaded',
          },
          setEntities(users),
        );
      } catch (error) {
        patchState(store, { loadingStatus: 'notLoaded' }, setError(error));
        throw error;
      }
    };

    const ensureLoaded = async () => {
      if (store.loadingStatus() === 'loaded') return;

      await loadUsers();
    };

    return { ensureLoaded };
  }),
);
