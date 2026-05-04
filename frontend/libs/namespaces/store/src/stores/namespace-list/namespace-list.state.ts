import { signalStoreFeature, withState } from '@ngrx/signals';
import { withEntities } from '@ngrx/signals/entities';
import { Namespace } from '../../models';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type NamespaceListState = {
  loadingStatus: 'not-loaded' | 'loading' | 'loaded';
};

export const initialState: NamespaceListState = {
  loadingStatus: 'not-loaded',
};

export function withNamespaceListState() {
  return signalStoreFeature(
    withState<NamespaceListState>(initialState),
    withEntities<Namespace>(),
    withErrorFeature(),
  );
}
