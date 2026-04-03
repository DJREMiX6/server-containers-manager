import { ContainerDto } from "../dtos/container.dto";

export type GetContainersResponse = {
    containers: ContainerDto[],
    totalCount: number;
}