import { signalStoreFeature, withState } from '@ngrx/signals';
import { withEntities } from '@ngrx/signals/entities';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';
import { User } from '../../models';

export type UsersListState = {
  loadingStatus: 'notLoaded' | 'loading' | 'loaded';
};

export const initialState: UsersListState = {
  loadingStatus: 'notLoaded',
};

export function withUsersListState() {
  return signalStoreFeature(
    withState<UsersListState>(initialState),
    withEntities<User>(),
    withErrorFeature(),
  );
}
