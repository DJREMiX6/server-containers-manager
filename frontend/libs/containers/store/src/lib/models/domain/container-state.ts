export type ContainerState = 
  | 'Created'
  | 'Running'
  | 'Paused'
  | 'Restarting'
  | 'Exited'
  | 'Removing'
  | 'Dead';