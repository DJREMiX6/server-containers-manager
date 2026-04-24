import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type CreateUserState = {
  requestStatus: 'idle' | 'pending' | 'fulfilled';
  generatedPassword: string | null;
  createdUserId: string | null;
};

export const initialState: CreateUserState = {
  requestStatus: 'idle',
  generatedPassword: null,
  createdUserId: null,
};

export function withCreateUserState() {
  return signalStoreFeature(
    withState<CreateUserState>(initialState),
    withErrorFeature(),
  );
}
