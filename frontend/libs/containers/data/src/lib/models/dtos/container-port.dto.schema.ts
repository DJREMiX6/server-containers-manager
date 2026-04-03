import * as z from "zod";
import { ContainerPortDto } from "./container-port.dto";

export const ContainerPortDtoSchema: z.ZodType<ContainerPortDto> = z.object({
  private: z.int().nonnegative().nonoptional(),
  public: z.int().nonnegative().nonoptional(),
});