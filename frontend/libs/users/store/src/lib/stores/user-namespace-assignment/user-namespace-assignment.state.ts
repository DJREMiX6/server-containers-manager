import { signalStoreFeature, withState } from '@ngrx/signals';
import { Namespace, User } from '../../models';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type UserNamespaceAssignmentState = {
  _allNamespaces: Namespace[];
  namespacesLoadingStatus: 'not-loaded' | 'loading' | 'loaded';
  updateStatus: 'unchanged' | 'pending' | 'changed' | 'error';
  user: User | null;
};

export const initialState: UserNamespaceAssignmentState = {
  _allNamespaces: [],
  namespacesLoadingStatus: 'not-loaded',
  updateStatus: 'unchanged',
  user: null,
};

export function withUserNamespaceAssignmentState() {
  return signalStoreFeature(
    withState<UserNamespaceAssignmentState>(initialState),
    withErrorFeature(),
  );
}
