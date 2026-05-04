import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { withNamespaceListState } from './namespace-list.state';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { firstValueFrom } from 'rxjs';
import { inject } from '@angular/core';
import { NamespacesService } from '@scm/namespaces/data';
import { namespaceDtoMapper } from '../../mappers';
import { setEntities } from '@ngrx/signals/entities';

export const NamespaceListStore = signalStore(
  withNamespaceListState(),
  withMethods((store, namespacesService = inject(NamespacesService)) => {
    const loadNamespaces = async () => {
      try {
        patchState(store, { loadingStatus: 'loading' }, clearError());

        const response = await firstValueFrom(
          namespacesService.getNamespaces(),
        );
        const namespaces = response.namespaces.map(namespaceDtoMapper);

        patchState(store, setEntities(namespaces), {
          loadingStatus: 'loaded',
        });
      } catch (error) {
        patchState(
          store,
          {
            loadingStatus: 'not-loaded',
          },
          setError(error),
        );
      }
    };

    const ensureLoaded = async () => {
      if (store.loadingStatus() !== 'not-loaded') return;

      await loadNamespaces();
    };

    return { ensureLoaded };
  }),
);
