import { ContainersSortByDto } from "../dtos/containers-sort-by.dto";

export type GetContainersRequest = {
  skip: number;
  take: number;
  sortBy: ContainersSortByDto;
  order: 'asc' | 'desc';
};