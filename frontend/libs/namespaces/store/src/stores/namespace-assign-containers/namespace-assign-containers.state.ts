import { signalStoreFeature, withState } from '@ngrx/signals';
import { Container } from '../../models';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type NamespaceAssignContainerState = {
  _associatedContainerIds: string[];
  containersLoadingStatus: 'notLoaded' | 'loading' | 'loaded' | 'error';
  namespaceContainersLoadingStatus:
    | 'notLoaded'
    | 'loading'
    | 'loaded'
    | 'error';
  containers: Container[];
  namespaceId: null | string;
  associatedContainersUpdateStatus:
    | 'unchanged'
    | 'pending'
    | 'changed'
    | 'error';
};

export const initialState: NamespaceAssignContainerState = {
  _associatedContainerIds: [],
  containersLoadingStatus: 'notLoaded',
  namespaceContainersLoadingStatus: 'notLoaded',
  containers: [],
  namespaceId: null,
  associatedContainersUpdateStatus: 'unchanged',
};

export function withNamespaceAssignContainersState() {
  return signalStoreFeature(
    withState<NamespaceAssignContainerState>(initialState),
    withErrorFeature(),
  );
}
