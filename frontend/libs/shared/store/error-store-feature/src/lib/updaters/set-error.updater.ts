import { ErrorState } from '../error-store-feature/error-store.feature';
import { parseError } from '../utils';

export function setError(error: unknown): ErrorState {
  return { error: parseError(error) };
}
