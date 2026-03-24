import { signalStoreFeature, withState } from '@ngrx/signals';
import { User } from '../models/user';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type AuthState = {
  isAuthenticated: boolean;
  user: User | null;
};

export const initialState: AuthState = {
  isAuthenticated: false,
  user: null,
};

export function withAuthState() {
  return signalStoreFeature(
    withState<AuthState>(initialState),
    withErrorFeature(),
  );
}
