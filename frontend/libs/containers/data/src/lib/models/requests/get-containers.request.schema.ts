import * as z from "zod";
import { GetContainersRequest } from "./get-containers.request";
import { OrderDtoSchema } from "../dtos/order.dto.schema";
import { ContainersSortByDtoSchema } from "../dtos/containers-sort-by.dto.schema";

export const GetContainersRequestSchema: z.ZodType<GetContainersRequest> = z.object({
  skip: z.int().gte(0).optional(),
  take: z.int().gte(0).optional(),
  order: OrderDtoSchema.optional(),
  sortBy: ContainersSortByDtoSchema.optional(),
}); 