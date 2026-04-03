import { ZodError } from "zod";
import { StoreErrorSeverity } from "../models/store-error-severity";

export type ValidationError = {
  readonly kind: 'validation';
  readonly severity: StoreErrorSeverity;
  readonly title: string;
  readonly summary: string;
  readonly raw: ZodError;
};