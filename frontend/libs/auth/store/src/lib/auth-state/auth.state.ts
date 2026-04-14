import { signalStoreFeature, withState } from '@ngrx/signals';
import { User } from '../models';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type AuthState = {
  requestStatus: 'idle' | 'pending' | 'fullfilled' | 'rejected';
  isAuthenticated: boolean;
  user: User | null;
};

export const initialState: AuthState = {
  requestStatus: 'idle',
  isAuthenticated: false,
  user: null,
};

export function withAuthState() {
  return signalStoreFeature(
    withState<AuthState>(initialState),
    withErrorFeature(),
  );
}
