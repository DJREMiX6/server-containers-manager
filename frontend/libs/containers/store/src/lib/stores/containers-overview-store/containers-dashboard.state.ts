import { signalStoreFeature, type, withState } from '@ngrx/signals';
import { withEntities } from '@ngrx/signals/entities';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';
import { ContainerSummary } from '../../models';

export type ContainersOverviewState = {
  readonly _containersToLoad: number;
  _loadedAt: Date | null;
  loadingStatus: 'notLoaded' | 'loading' | 'loaded';
};

export const initialState: ContainersOverviewState = {
  _containersToLoad: 4,
  _loadedAt: null,
  loadingStatus: 'notLoaded',
};

export function withContainersOverviewState() {
  return signalStoreFeature(
    withState<ContainersOverviewState>(initialState),
    withErrorFeature(),
    withEntities<ContainerSummary>(),
  );
}
