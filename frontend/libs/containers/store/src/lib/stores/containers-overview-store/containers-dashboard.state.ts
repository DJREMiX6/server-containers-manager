import { signalStoreFeature, withState } from '@ngrx/signals';
import { ContainerSummary } from '../../models';

export type ContainersOverviewState = {
  readonly _containersToLoad: number;
  _loadedAt: Date | null;
  loadingStatus: 'notLoaded' | 'loading' | 'loaded';
  containers: ContainerSummary[];
  error: Error | null;
};

export const initialState: ContainersOverviewState = {
  _containersToLoad: 4,
  _loadedAt: null,
  loadingStatus: 'notLoaded',
  containers: [],
  error: null,
};

export function withContainersOverviewState() {
  return signalStoreFeature(withState<ContainersOverviewState>(initialState));
}
