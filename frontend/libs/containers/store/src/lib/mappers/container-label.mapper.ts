import { ContainerLabelDto } from "@scm/containers/data";
import { ContainerLabels } from "../models";

export function containerLabelsMapper(dtos: ContainerLabelDto[]): ContainerLabels {
    const parsedDtos = dtos.map((dto) => [dto.key, dto.value]);
    return Object.fromEntries(parsedDtos);
}