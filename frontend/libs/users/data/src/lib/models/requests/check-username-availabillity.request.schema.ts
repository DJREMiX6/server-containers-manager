import { z } from 'zod';
import { CheckUsernameAvailabilityRequest } from './check-username-availabillity.request';

export const CheckUsernameAvailabilityRequestSchema: z.ZodType<CheckUsernameAvailabilityRequest> =
  z.object({
    username: z.string().min(3).nonempty().nonoptional(),
  });
