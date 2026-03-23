import { ContainerLabels } from "./container-label";
import { ContainerPort } from "./container-port";
import { ContainerState } from "./container-state";
import { Namespace } from "./namespace";

export type ContainerSummary = {
    id: string;
    name: string;
    createdAt: Date;
    state: ContainerState;
    labels: ContainerLabels;
    ports: ContainerPort[];
    namespaces: Namespace[]
};