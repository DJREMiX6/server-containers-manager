export type UnknownError = {
  readonly kind: 'unknown';
  readonly severity: 'error';
  readonly title: string;
  readonly summary: string;
  readonly raw: unknown;
};