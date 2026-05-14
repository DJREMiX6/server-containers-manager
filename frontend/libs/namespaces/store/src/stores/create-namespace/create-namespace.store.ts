import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { withCreateNamespaceState } from './create-namespace.state';
import { CreateNamespaceRequest as LocalCreateNamespaceRequest } from '../../models';
import { clearError, setError } from '@scm/shared/store/error-store-feature';
import { firstValueFrom } from 'rxjs';
import { inject } from '@angular/core';
import { NamespacesService } from '@scm/namespaces/data';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';

export const CreateNamespaceStore = signalStore(
  withCreateNamespaceState(),
  withMethods((store, namespacesService = inject(NamespacesService)) => {
    const createNamespace = async (request: LocalCreateNamespaceRequest) => {
      try {
        patchState(store, { requestStatus: 'pending' }, clearError());

        const response = await firstValueFrom(
          namespacesService.createNamespace({
            name: request.name,
          }),
        );

        patchState(store, {
          requestStatus: 'fulfilled',
          createdNamespaceId: response.namespaceId,
        });
      } catch (error) {
        patchState(store, { requestStatus: 'idle' }, setError(error));
      }
    };

    const checkNameAvailability = async (
      name: string,
    ): Promise<{ isAvailable: boolean }> => {
      try {
        patchState(store, clearError());

        await firstValueFrom(namespacesService.checkNameAvailability({ name }));

        return { isAvailable: true };
      } catch (error) {
        if (
          error instanceof HttpErrorResponse &&
          error.status == HttpStatusCode.Conflict
        ) {
          return { isAvailable: false };
        }

        patchState(store, setError(error));
        throw error;
      }
    };

    return { createNamespace, checkNameAvailability };
  }),
);
