import { ContainerSummary } from "../models";
import { ContainerDto } from "@scm/containers/data";
import { namespacesMapper } from "./namespace.mapper";
import { containerPortsMapper } from "./container-port.mapper";
import { containerLabelsMapper } from "./container-label.mapper";

export function containerSummaryMapper(dto: ContainerDto): ContainerSummary {
    return {
      id: dto.id,
      name: dto.name,
      createdAt: new Date(dto.createdAt),
      updatedAt: new Date(dto.updatedAt),
      state: dto.state,
      labels: containerLabelsMapper(dto.labels),
      ports: containerPortsMapper(dto.ports),
      namespaces: namespacesMapper(dto.namespaces),
      updating: false,
    };
}

export function containersSummaryMapper(dtos: ContainerDto[]): ContainerSummary[] {
    return dtos.map(containerSummaryMapper);
}