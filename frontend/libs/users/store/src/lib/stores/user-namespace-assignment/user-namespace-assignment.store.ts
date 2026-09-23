import {
  patchState,
  signalStore,
  withComputed,
  withMethods,
} from '@ngrx/signals';
import {
  initialState,
  withUserNamespaceAssignmentState,
} from './user-namespace-assignment.state';
import { Namespace, User } from '../../models';
import { computed, inject } from '@angular/core';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { firstValueFrom } from 'rxjs';
import { NamespacesService } from '@scm/namespaces/data';
import { namespaceInfoDtoMapper } from '../../mappers';
import { UsersService } from '@scm/users/data';

export const UserNamespaceAssignmentStore = signalStore(
  withUserNamespaceAssignmentState(),
  withComputed((store) => ({
    associatedNamespaces: computed(() => {
      const user = store.user();
      if (!user) return [];

      const namespaces = store._allNamespaces();
      if (namespaces.length === 0) return [];

      const userNamespaceIds = user.namespaces.map((n) => n.id);

      return namespaces.filter((n) => userNamespaceIds.includes(n.id));
    }),
    unassociatedNamespaces: computed(() => {
      const user = store.user();
      if (!user) return [];

      const namespaces = store._allNamespaces();
      if (namespaces.length === 0) return [];

      const userNamespaceIds = user.namespaces.map((n) => n.id);

      return namespaces.filter((n) => !userNamespaceIds.includes(n.id));
    }),
  })),
  withMethods(
    (
      store,
      namespacesService = inject(NamespacesService),
      usersService = inject(UsersService),
    ) => {
      const _loadNamespaces = async () => {
        try {
          patchState(store, {
            namespacesLoadingStatus: 'loading',
          });

          const response = await firstValueFrom(
            namespacesService.getNamespaces(),
          );
          const namespaces = response.namespaces.map(namespaceInfoDtoMapper);

          patchState(store, {
            _allNamespaces: namespaces,
            namespacesLoadingStatus: 'loaded',
          });
        } catch (error) {
          patchState(store, setError(error), {
            namespacesLoadingStatus: 'not-loaded',
          });
        }
      };

      const ensureLoaded = async (user: User) => {
        patchState(store, {
          user,
        });

        if (store.namespacesLoadingStatus() !== 'loaded')
          await _loadNamespaces();
      };

      const updateAssociatedNamespaces = async (
        associatedNamespaces: Namespace[],
      ) => {
        try {
          const user = store.user();
          if (!user) throw new Error('User is not set.');

          const associatedNamespaceIds = associatedNamespaces.map((n) => n.id);

          patchState(store, clearError(), { updateStatus: 'pending' });

          await firstValueFrom(
            usersService.updateUserNamespaces({
              userId: user.id,
              namespacesIds: associatedNamespaceIds,
            }),
          );

          patchState(store, {
            updateStatus: 'changed',
          });
        } catch (error) {
          patchState(store, setError(error), { updateStatus: 'error' });
        }
      };

      const reset = () => {
        patchState(store, { ...initialState });
      };

      return { ensureLoaded, updateAssociatedNamespaces, reset };
    },
  ),
);
