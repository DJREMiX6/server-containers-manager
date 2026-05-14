import { signalStoreFeature, withState } from '@ngrx/signals';
import { withErrorFeature } from '@scm/shared/store/error-store-feature';

export type CreateNamespaceState = {
  requestStatus: 'idle' | 'pending' | 'fulfilled';
  createdNamespaceId: string | null;
};

export const initialState: CreateNamespaceState = {
  requestStatus: 'idle',
  createdNamespaceId: null,
};

export function withCreateNamespaceState() {
  return signalStoreFeature(
    withState<CreateNamespaceState>(initialState),
    withErrorFeature(),
  );
}
