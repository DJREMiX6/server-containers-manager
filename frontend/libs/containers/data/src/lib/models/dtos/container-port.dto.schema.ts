import * as z from "zod";
import { ContainerPortDto } from "./container-port.dto";

export const ContainerPortDtoSchema: z.ZodType<ContainerPortDto> = z.object({
    private: z.number().nonnegative().nonoptional(),
    public: z.number().nonnegative().nonoptional(),
});