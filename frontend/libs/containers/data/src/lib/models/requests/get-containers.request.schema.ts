import * as z from "zod";
import { GetContainersRequest } from "./get-containers.request";
import { OrderDtoSchema } from "../dtos/order.dto.schema";
import { ContainersSortByDtoSchema } from "../dtos/containers-sort-by.dto.schema";

export const GetContainersRequestSchema: z.ZodType<GetContainersRequest> = z.object({
    order: OrderDtoSchema,
    sortBy: ContainersSortByDtoSchema
}); 