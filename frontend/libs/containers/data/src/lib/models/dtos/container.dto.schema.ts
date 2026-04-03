import * as z from "zod";
import { ContainerDto } from "./container.dto";
import { ContainerStateDtoSchema } from "./container-state.dto.schema";
import { ContainerLabelDtoSchema } from "./container-label.dto.schema";
import { ContainerPortDtoSchema } from "./container-port.dto.schema";
import { NamespaceDtoSchema } from "./namespace.dto.schema";

export const ContainerDtoSchema: z.ZodType<ContainerDto> = z.object({
  id: z.string().length(64).nonempty().nonoptional(),
  name: z.string().nonempty(),
  createdAt: z.iso.datetime().nonempty(),
  updatedAt: z.iso.datetime().nonempty(),
  state: ContainerStateDtoSchema,
  labels: z.array(ContainerLabelDtoSchema),
  ports: z.array(ContainerPortDtoSchema),
  namespaces: z.array(NamespaceDtoSchema),
});