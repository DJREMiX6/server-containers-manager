import { OutputEmitterRef } from '@angular/core';

export interface DialogComponent<TCompleted = void, TCanceled = void> {
  readonly operationCanceled: OutputEmitterRef<TCanceled>;
  readonly operationCompleted: OutputEmitterRef<TCompleted>;
}
