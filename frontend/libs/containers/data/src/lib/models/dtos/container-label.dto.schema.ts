import * as z from "zod";
import { ContainerLabelDto } from "./container-label.dto";

export const ContainerLabelDtoSchema: z.ZodType<ContainerLabelDto> = z.object({
    key: z.string().nonempty(),
    value: z.string().nonempty()
});