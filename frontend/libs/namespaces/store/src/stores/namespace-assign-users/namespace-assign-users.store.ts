import {
  patchState,
  signalStore,
  withComputed,
  withMethods,
} from '@ngrx/signals';
import { withAssignUsersState } from './namespace-assign-users.state';
import { computed, inject } from '@angular/core';
import { UsersService } from '@scm/users/data';
import { NamespacesService } from '@scm/namespaces/data';
import { firstValueFrom } from 'rxjs';
import { getUsersResponseMapper, User } from '@scm/users/store';
import { setError } from '@scm/shared/store/error-store-feature';

export const NamespaceAssignUsersStore = signalStore(
  withAssignUsersState(),
  withComputed((store) => ({
    assignedUsers: computed((): User[] => {
      const users = store.users();
      const assignedUserIds = store._assignedUserIds();

      if (
        store.users().length === 0 ||
        store._namespaceUsersLoadingStatus() !== 'loaded' ||
        store._usersLoadingStatus() !== 'loaded'
      )
        return [];

      return users.filter((assignedUser) =>
        assignedUserIds.some((userId) => userId === assignedUser.id),
      );
    }),
    unassignedUsers: computed((): User[] => {
      const users = store.users();
      const assignedUserIds = store._assignedUserIds();

      if (
        store.users().length === 0 ||
        store._namespaceUsersLoadingStatus() !== 'loaded' ||
        store._usersLoadingStatus() !== 'loaded'
      )
        return [];

      return users.filter((assignedUser) =>
        assignedUserIds.some((userId) => userId !== assignedUser.id),
      );
    }),
  })),
  withMethods(
    (
      store,
      usersService = inject(UsersService),
      namespacesService = inject(NamespacesService),
    ) => {
      const ensureUsersLoaded = async (): Promise<void> => {
        try {
          if (store._usersLoadingStatus() !== 'notLoaded') return;

          patchState(store, { _usersLoadingStatus: 'loading' });

          const response = await firstValueFrom(usersService.getUsers());
          const users = getUsersResponseMapper(response);

          patchState(store, { users: users, _usersLoadingStatus: 'loaded' });
        } catch (error) {
          patchState(store, setError(error), {
            _usersLoadingStatus: 'notLoaded',
          });
        }
      };

      const selectNamespace = async (namespaceId: string): Promise<void> => {
        if (namespaceId === store.namespaceId()) return;

        patchState(store, { namespaceId });
        await ensureUsersLoaded();
        await ensureNamespaceUsersLoaded();
      };

      const ensureNamespaceUsersLoaded = async (): Promise<void> => {
        try {
          const namespaceId = store.namespaceId();
          if (namespaceId === null) return;

          patchState(store, { _namespaceUsersLoadingStatus: 'loading' });

          const response = await firstValueFrom(
            namespacesService.getNamespaceAssignedUsers({
              namespaceId,
            }),
          );
          const assignedUserIds = response.associatedUsers.map(
            (user) => user.id,
          );

          patchState(store, {
            _namespaceUsersLoadingStatus: 'loaded',
            _assignedUserIds: assignedUserIds,
          });
        } catch (error) {
          patchState(store, setError(error), {
            _namespaceUsersLoadingStatus: 'notLoaded',
          });
        }
      };

      return { selectNamespace };
    },
  ),
);
