export type ContainerStateDto =
  | 'Created'
  | 'Running'
  | 'Paused'
  | 'Restarting'
  | 'Exited'
  | 'Removing'
  | 'Dead';
