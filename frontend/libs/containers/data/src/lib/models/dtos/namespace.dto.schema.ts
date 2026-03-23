import * as z from "zod";
import { NamespaceDto } from "./namespace.dto";

export const NamespaceDtoSchema: z.ZodType<NamespaceDto> = z.object({
    id: z.guid().nonempty().nonoptional(),
    name: z.string().nonempty().nonoptional()
});