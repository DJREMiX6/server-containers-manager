export type NetworkError = {
  readonly kind: 'network';
  readonly severity: 'error';
  readonly title: string;
  readonly summary: string;
  readonly raw: Error;
};