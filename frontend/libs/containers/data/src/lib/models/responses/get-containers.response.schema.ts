import * as z from "zod";
import { GetContainersResponse } from "./get-containers.response";
import { ContainerDtoSchema } from "../dtos/container.dto.schema";

export const GetContainersResponseSchema: z.ZodType<GetContainersResponse> = z.object({
  containers: z.array(ContainerDtoSchema),
  totalCount: z.int().gte(0).nonnegative().nonoptional(),
});