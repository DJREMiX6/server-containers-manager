export type ContainerOverviewInfo = {
  id: string;
  name: string;
  state: "created" | "running" | "paused" | "restarting" | "exited" | "removing" | "dead";
  namespaces: string[];
};