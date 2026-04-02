import { ContainerStateDto } from "./container-state.dto";
import { ContainerLabelDto } from "./container-label.dto";
import { ContainerPortDto } from "./container-port.dto";
import { NamespaceDto } from "./namespace.dto"

export type ContainerDto = {
  id: string;
  name: string;
  state: ContainerStateDto;
  createdAt: string;
  updatedAt: string;
  labels: ContainerLabelDto[];
  ports: ContainerPortDto[];
  namespaces: NamespaceDto[];
};