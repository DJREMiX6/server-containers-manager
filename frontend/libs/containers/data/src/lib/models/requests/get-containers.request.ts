import { ContainersSortByDto } from "../dtos/containers-sort-by.dto";

export type GetContainersRequest = {
    sortBy: ContainersSortByDto;
    order: "asc" | "desc";
}