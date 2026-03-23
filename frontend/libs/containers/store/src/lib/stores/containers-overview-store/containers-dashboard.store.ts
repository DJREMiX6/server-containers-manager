import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { setError, clearError } from '@scm/shared/store/error-store-feature';
import { withContainersOverviewState } from './containers-dashboard.state';
import { inject } from '@angular/core';
import { ContainersService, GetContainersRequest } from '@scm/containers/data';
import { firstValueFrom } from 'rxjs';
import { containersSummaryMapper } from '../../mappers';

export const ContainersOverviewStore = signalStore(
  withContainersOverviewState(),
  withMethods((store, containersService = inject(ContainersService)) => {
    const loadContainers = async () => {
      try {
        patchState(store, { loadingStatus: 'loading' }, clearError());

        const request: GetContainersRequest = {
          skip: 0,
          take: store._containersToLoad(),
          order: 'desc',
          sortBy: 'created',
        };

        const response = await firstValueFrom(
          containersService.getContainers(request),
        );

        patchState(store, {
          containers: containersSummaryMapper(response.containers),
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

    return { ensureLoaded };
  }),
);
