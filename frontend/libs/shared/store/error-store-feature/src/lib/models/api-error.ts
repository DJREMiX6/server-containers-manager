import { HttpErrorResponse } from "@angular/common/http";
import { StoreErrorSeverity } from "./store-error-severity";

export type ApiError = {
  readonly kind: 'api';
  readonly severity: StoreErrorSeverity;
  readonly title: string;
  readonly summary: string;
  readonly raw: HttpErrorResponse;
};