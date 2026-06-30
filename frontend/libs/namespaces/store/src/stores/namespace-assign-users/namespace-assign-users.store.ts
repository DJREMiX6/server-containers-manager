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
import { clearError, setError } from '@scm/shared/store/error-store-feature';

export const NamespaceAssignUsersStore = signalStore(
  withAssignUsersState(),
  withComputed((store) => ({
    associatedUsers: computed((): User[] => {
      const users = store.users();
      const associatedUserIds = store._associatedUserIds();

      if (
        store.users().length === 0 ||
        store.namespaceUsersLoadingStatus() !== 'loaded' ||
        store.usersLoadingStatus() !== 'loaded'
      )
        return [];

      return users.filter((user) =>
        associatedUserIds.some(
          (associatedUserId) => associatedUserId === user.id,
        ),
      );
    }),
    unassociatedUsers: computed((): User[] => {
      const users = store.users();
      const associatedUserIds = store._associatedUserIds();

      if (
        store.users().length === 0 ||
        store.namespaceUsersLoadingStatus() !== 'loaded' ||
        store.usersLoadingStatus() !== 'loaded'
      )
        return [];

      return users.filter(
        (user) =>
          !associatedUserIds.some(
            (associatedUserId) => associatedUserId === user.id,
          ),
      );
    }),
  })),
  withMethods(
    (
      store,
      usersService = inject(UsersService),
      namespacesService = inject(NamespacesService),
    ) => {
      const _ensureUsersLoaded = async (): Promise<void> => {
        try {
          if (store.usersLoadingStatus() !== 'notLoaded') return;

          patchState(store, clearError(), { usersLoadingStatus: 'loading' });

          const response = await firstValueFrom(usersService.getUsers());
          const users = getUsersResponseMapper(response).filter(
            (user) => !user.roles.includes('Admin'),
          );

          patchState(store, { users: users, usersLoadingStatus: 'loaded' });
        } catch (error) {
          patchState(store, setError(error), {
            usersLoadingStatus: 'notLoaded',
          });
        }
      };

      const _ensureNamespaceUsersLoaded = async (): Promise<void> => {
        try {
          const namespaceId = store.namespaceId();
          if (namespaceId === null) return;

          patchState(store, clearError(), {
            namespaceUsersLoadingStatus: 'loading',
          });

          const response = await firstValueFrom(
            namespacesService.getNamespaceAssignedUsers({
              namespaceId,
            }),
          );
          const assignedUserIds = response.associatedUsers.map(
            (user) => user.id,
          );

          patchState(store, {
            namespaceUsersLoadingStatus: 'loaded',
            _associatedUserIds: assignedUserIds,
          });
        } catch (error) {
          patchState(store, setError(error), {
            namespaceUsersLoadingStatus: 'notLoaded',
          });
        }
      };

      const selectNamespace = async (
        namespaceId: string | null,
      ): Promise<void> => {
        if (namespaceId === store.namespaceId()) return;

        patchState(store, { namespaceId });

        if (namespaceId === null) return;

        await _ensureUsersLoaded();
        await _ensureNamespaceUsersLoaded();
      };

      const updateAssociatedUsers = async (associatedUsers: User[]) => {
        try {
          const namespaceId = store.namespaceId();
          if (!namespaceId) return;

          patchState(store, clearError(), {
            associatedUsersUpdateStatus: 'pending',
          });

          const associatedUserIds = associatedUsers.map((user) => user.id);

          await firstValueFrom(
            namespacesService.updateNamespaceUsers({
              namespaceId,
              data: {
                associatedUserIds,
              },
            }),
          );

          patchState(store, { associatedUsersUpdateStatus: 'changed' });
        } catch (error) {
          patchState(store, setError(error), {
            associatedUsersUpdateStatus: 'error',
          });
        }
      };

      const resetAssociatedUsers = () => {
        if (store.namespaceId() === null) return;
        patchState(store, {
          _associatedUserIds: [...store._associatedUserIds()],
        });
      };

      return { selectNamespace, updateAssociatedUsers, resetAssociatedUsers };
    },
  ),
);
