import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';
import { ContainerSummary } from '../../models';

export type ContainersOverviewState = {
  readonly _containersToLoad: number;
  _loadedAt: Date | null;
  loadingStatus: 'notLoaded' | 'loading' | 'loaded';
  _containers: ContainerSummary[];
};

export const initialState: ContainersOverviewState = {
  _containersToLoad: 4,
  _loadedAt: null,
  loadingStatus: 'notLoaded',
  _containers: [],
};

export function withContainersOverviewState() {
  return signalStoreFeature(
    withState<ContainersOverviewState>(initialState),
    withErrorFeature(), //Use entities store
  );
}
