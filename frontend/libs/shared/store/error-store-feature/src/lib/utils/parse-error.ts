import { ZodError } from 'zod';
import {
  ApiError,
  NetworkError,
  StoreError,
  UnknownError,
  ValidationError,
} from '../models';
import { HttpErrorResponse } from '@angular/common/http';

export function parseError(error: unknown): StoreError {
  if (error instanceof ZodError) return parseValidationError(error);

  if (error instanceof HttpErrorResponse) return parseHttpError(error);

  if (error instanceof Error && error.message?.includes('Network'))
    return parseNetworkError(error);

  return parseUnknownError(error);
}

function parseValidationError(error: ZodError): ValidationError {
  const issues = error.issues.map((issue) => issue.message).join('; ');
  return {
    kind: 'validation',
    severity: 'error',
    title: 'Validation Error',
    summary: issues || 'Validation failed',
    raw: error,
  };
}

function parseHttpError(error: HttpErrorResponse): ApiError | NetworkError {
  if (error.status === 0) return parseNetworkError(error);

  return parseApiError(error);
}

function parseNetworkError(error: HttpErrorResponse | Error): NetworkError {
  return {
    kind: 'network',
    severity: 'error',
    title: 'Network Error',
    summary:
      error instanceof HttpErrorResponse
        ? 'Unable to reach the server. Please check your internet connection.'
        : error.message,
    raw: error,
  };
}

function parseApiError(error: HttpErrorResponse): ApiError {
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

function parseUnknownError(error: unknown): UnknownError {
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
