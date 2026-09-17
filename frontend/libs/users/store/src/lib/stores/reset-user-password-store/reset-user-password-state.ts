import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type ResetUserPasswordState = {
  resetStatus: 'idle' | 'pending' | 'successful' | 'error';
  generatedPassword: string | null;
  userIdToReset: string | null;
};

export const initialState: ResetUserPasswordState = {
  resetStatus: 'idle',
  generatedPassword: null,
  userIdToReset: null,
};

export function withResetUserPasswordState() {
  return signalStoreFeature(
    withState<ResetUserPasswordState>(initialState),
    withErrorFeature(),
  );
}
