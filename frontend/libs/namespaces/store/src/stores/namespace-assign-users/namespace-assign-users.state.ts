import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';
import { User } from '@scm/users/store';

export type AssignUsersState = {
  usersLoadingStatus: 'notLoaded' | 'loading' | 'loaded' | 'error';
  namespaceUsersLoadingStatus: 'notLoaded' | 'loading' | 'loaded' | 'error';
  _associatedUserIds: string[];
  users: User[];
  namespaceId: null | string;
  associatedUsersUpdateStatus: 'unchanged' | 'pending' | 'changed' | 'error';
};

export const initialState: AssignUsersState = {
  usersLoadingStatus: 'notLoaded',
  namespaceUsersLoadingStatus: 'notLoaded',
  _associatedUserIds: [],
  users: [],
  namespaceId: null,
  associatedUsersUpdateStatus: 'unchanged',
};

export function withAssignUsersState() {
  return signalStoreFeature(
    withState<AssignUsersState>(initialState),
    withErrorFeature(),
  );
}
