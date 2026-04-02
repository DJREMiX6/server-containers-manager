import {
  patchState,
  signalStore,
  withComputed,
  withMethods,
} from '@ngrx/signals';
import { setError, clearError } from '@scm/shared/store/error-store-feature';
import { withContainersOverviewState } from './containers-dashboard.state';
import { computed, inject } from '@angular/core';
import { ContainersService, GetContainersRequest } from '@scm/containers/data';
import { firstValueFrom } from 'rxjs';
import { containersSummaryMapper } from '../../mappers';

export const ContainersOverviewStore = signalStore(
  withContainersOverviewState(),
  withComputed((store) => ({
    containers: computed(() =>
      store
        ._containers()
        .sort((a, b) => b.updatedAt.getTime() - a.updatedAt.getTime()),
    ),
  })),
  withMethods((store, containersService = inject(ContainersService)) => {
    const loadContainers = async () => {
      try {
        patchState(store, { loadingStatus: 'loading' }, clearError());

        const request: GetContainersRequest = {
          skip: 0,
          take: store._containersToLoad(),
          order: 'desc',
          sortBy: 'updated',
        };

        const response = await firstValueFrom(
          containersService.getContainers(request),
        );

        patchState(store, {
          _containers: containersSummaryMapper(response.containers),
          _loadedAt: new Date(),
          loadingStatus: 'loaded',
        });
      } catch (error) {
        patchState(
          store,
          {
            loadingStatus: 'notLoaded',
          },
          setError(error),
        );
        throw error;
      }
    };

    const ensureLoaded = async () => {
      if (store.loadingStatus() === 'loaded') return;

      await loadContainers();
    };

    const startContainer = async (containerId: string) => {
      try {
        if (store.loadingStatus() !== 'loaded') return;

        const container = store._containers().find((c) => c.id === containerId);
        if (!container)
          throw new Error(`Missing container with id ${containerId}.`);

        patchState();
        await firstValueFrom(containersService.startContainer({ containerId }));
      } catch (error) {
        patchState(store, setError(error));
        throw error;
      }
    };

    return { ensureLoaded };
  }),
);
