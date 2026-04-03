import * as z from "zod";

export const ContainerIdSchema = z.string().length(64).nonempty().nonoptional();