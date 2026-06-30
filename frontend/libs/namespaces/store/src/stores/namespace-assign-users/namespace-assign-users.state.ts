import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';
import { User } from '@scm/users/store';

export type AssignUsersState = {
  _usersLoadingStatus: 'notLoaded' | 'loading' | 'loaded';
  _namespaceUsersLoadingStatus: 'notLoaded' | 'loading' | 'loaded';
  _assignedUserIds: string[];
  users: User[];
  namespaceId: null | string;
};

export const initialState: AssignUsersState = {
  _usersLoadingStatus: 'notLoaded',
  _namespaceUsersLoadingStatus: 'notLoaded',
  _assignedUserIds: [],
  users: [],
  namespaceId: null,
};

export function withAssignUsersState() {
  return signalStoreFeature(
    withState<AssignUsersState>(initialState),
    withErrorFeature(),
  );
}
