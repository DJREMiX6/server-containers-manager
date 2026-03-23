import * as z from "zod";
import { OrderDto } from "./order.dto";

export const OrderDtoSchema: z.ZodType<OrderDto> = z.literal([
    "asc",
    "desc"
]);