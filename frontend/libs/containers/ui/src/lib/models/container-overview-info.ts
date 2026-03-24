import { ContainerState } from './container-state';

export type ContainerOverviewInfo = {
  id: string;
  name: string;
  state: ContainerState;
  namespaces: string[];
};
