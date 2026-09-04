import {
  patchState,
  signalStore,
  withComputed,
  withMethods,
} from '@ngrx/signals';
import { withNamespaceAssignContainersState } from './namespace-assign-containers.state';
import { firstValueFrom } from 'rxjs';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { ContainersService } from '@scm/containers/data';
import { computed, inject } from '@angular/core';
import { NamespacesService } from '@scm/namespaces/data';
import { containersSummaryMapper } from '@scm/containers/store';
import { Container } from '../../models';

export const NamespaceAssignContainersStore = signalStore(
  withNamespaceAssignContainersState(),
  withComputed((store) => ({
    associatedContainers: computed((): Container[] => {
      const containers = store.containers();
      const associatedContainerIds = store._associatedContainerIds();

      if (
        containers.length === 0 ||
        store.namespaceContainersLoadingStatus() !== 'loaded' ||
        store.containersLoadingStatus() !== 'loaded'
      )
        return [];

      return containers.filter((container) =>
        associatedContainerIds.some(
          (associatedContainerId) => associatedContainerId === container.id,
        ),
      );
    }),
    unassociatedContainers: computed((): Container[] => {
      const containers = store.containers();
      const associatedContainerIds = store._associatedContainerIds();

      if (
        containers.length === 0 ||
        store.namespaceContainersLoadingStatus() !== 'loaded' ||
        store.containersLoadingStatus() !== 'loaded'
      )
        return [];

      return containers.filter(
        (container) =>
          !associatedContainerIds.some(
            (associatedContainerId) => associatedContainerId === container.id,
          ),
      );
    }),
  })),
  withMethods(
    (
      store,
      namespacesService = inject(NamespacesService),
      containersService = inject(ContainersService),
    ) => {
      const _ensureContainersLoaded = async (): Promise<void> => {
        try {
          if (store.containersLoadingStatus() !== 'notLoaded') return;

          patchState(store, clearError(), {
            containersLoadingStatus: 'loading',
          });

          const response = await firstValueFrom(
            containersService.getContainers({}),
          );
          const containers = containersSummaryMapper(response.containers);

          patchState(store, {
            containers,
            containersLoadingStatus: 'loaded',
          });
        } catch (error) {
          patchState(store, setError(error), {
            containersLoadingStatus: 'notLoaded',
          });
        }
      };

      const _ensureNamespaceContainersLoaded = async (): Promise<void> => {
        try {
          const namespaceId = store.namespaceId();
          if (namespaceId === null) return;

          patchState(store, clearError(), {
            namespaceContainersLoadingStatus: 'loading',
          });

          const response = await firstValueFrom(
            namespacesService.getNamespaceAssociatedContainers({
              namespaceId,
            }),
          );
          const associatedContainersIds = response.associatedContainers.map(
            (container) => container.id,
          );

          patchState(store, {
            namespaceContainersLoadingStatus: 'loaded',
            _associatedContainerIds: associatedContainersIds,
          });
        } catch (error) {
          patchState(store, setError(error), {
            namespaceContainersLoadingStatus: 'notLoaded',
          });
        }
      };
      ///////////////////////////////////////////////////////////////TODO: CHECK FOR ASSOCIATED CONTAINERS WHICH ARE EMPTY

      const selectNamespace = async (
        namespaceId: string | null,
      ): Promise<void> => {
        if (namespaceId === store.namespaceId()) return;

        patchState(store, { namespaceId });

        if (namespaceId === null) return;

        await _ensureContainersLoaded();
        await _ensureNamespaceContainersLoaded();
      };

      const updateAssociatedContainers = async (containers: Container[]) => {
        try {
          const namespaceId = store.namespaceId();
          if (!namespaceId) throw new Error('NamespaceId not set.');

          const containersIds = containers.map((c) => c.id);

          patchState(store, {
            associatedContainersUpdateStatus: 'pending',
            error: null,
          });

          await firstValueFrom(
            namespacesService.updateNamespaceContainers({
              namespaceId,
              data: {
                associatedContainersIds: containersIds,
              },
            }),
          );

          patchState(store, {
            associatedContainersUpdateStatus: 'changed',
            _associatedContainerIds: containersIds,
          });
        } catch (error) {
          patchState(store, setError(error), {
            associatedContainersUpdateStatus: 'error',
          });
        }
      };

      const resetAssociatedContainers = async () => {
        if (store.namespaceId() === null) return;
        patchState(store, {
          _associatedContainerIds: [...store._associatedContainerIds()],
        });
      };

      return {
        selectNamespace,
        updateAssociatedContainers,
        resetAssociatedContainers,
      };
    },
  ),
);
