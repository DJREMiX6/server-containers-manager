import { z } from 'zod';
import { CheckNameAvailabilityRequest } from './check-name-availability.request';

export const CheckNameAvailabilityRequestSchema: z.ZodType<CheckNameAvailabilityRequest> =
  z.object({
    name: z.string().nonoptional(),
  });
