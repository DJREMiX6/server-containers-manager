import {patchState, signalStoreFeature, withComputed, withMethods, withState} from "@ngrx/signals";
import { computed } from "@angular/core";
import { StoreError } from "../models";
import { parseError } from "../utils";


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
    withMethods((store) => ({
      setError(error: unknown): void {
        const classified = parseError(error);
        console.error(`[${classified.kind}] ${classified.title}: ${classified.summary}`, classified.raw);
        patchState(store, { error: classified });
      },
      clearError(): void {
        patchState(store, { error: null });
      },
    })),
  );
}
