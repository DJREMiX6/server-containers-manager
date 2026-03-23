import { ErrorState } from '../error-store-feature/error-store.feature';

export function clearError(): ErrorState {
  return { error: null };
}
