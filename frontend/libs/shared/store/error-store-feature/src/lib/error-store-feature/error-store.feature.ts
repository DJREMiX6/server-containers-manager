import { signalStoreFeature, withComputed, withState } from '@ngrx/signals';
import { computed } from '@angular/core';
import { StoreError } from '../models';

export type ErrorState = {
  error: StoreError | null;
};

export const initialErrorState: ErrorState = {
  error: null,
};

export function withErrorFeature() {
  return signalStoreFeature(
    withState<ErrorState>(initialErrorState),
    withComputed(({ error }) => ({
      hasError: computed(() => error() !== null),
    })),
  );
}
