import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type DeleteUserState = {
  deleteUserStatus: 'idle' | 'pending' | 'completed' | 'error';
};

export const initialState: DeleteUserState = {
  deleteUserStatus: 'idle',
};

export function withDeleteUserState() {
  return signalStoreFeature(
    withState<DeleteUserState>(initialState),
    withErrorFeature(),
  );
}
