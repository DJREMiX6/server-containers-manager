import { ZodError } from "zod";
import { StoreError } from "../models";
import { HttpErrorResponse } from "@angular/common/http";

export function parseError(error: unknown): StoreError {
  if (error instanceof ZodError) {
    const issues = error.issues.map((issue) => issue.message).join('; ');
    return {
      kind: 'validation',
      severity: 'error',
      title: 'Validation Error',
      summary: issues || 'Validation failed',
      raw: error,
    };
  }

  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return {
        kind: 'network',
        severity: 'error',
        title: 'Network Error',
        summary:
          'Unable to reach the server. Please check your internet connection.',
        raw: error,
      };
    }

    const body = error.error;
    const title = body?.title ?? `Server Error (${error.status})`;
    const summary =
      body?.detail ?? error.message ?? 'An unexpected server error occurred.';
    return {
      kind: 'api',
      severity: error.status >= 500 ? 'error' : 'warning',
      title,
      summary,
      raw: error,
    };
  }

  if (error instanceof Error && error.message?.includes('Network')) {
    return {
      kind: 'network',
      severity: 'error',
      title: 'Network Error',
      summary: error.message,
      raw: error,
    };
  }

  return {
    kind: 'unknown',
    severity: 'error',
    title: 'Unexpected Error',
    summary:
      error instanceof Error
        ? error.message
        : 'An unexpected error has occurred.',
    raw: error,
  };
}