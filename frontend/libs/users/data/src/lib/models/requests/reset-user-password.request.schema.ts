import { z } from 'zod';
import { ResetUserPasswordRequest } from './reset-user-password.request';

export const ResetUserPasswordRequestSchema: z.ZodType<ResetUserPasswordRequest> =
  z.object({
    userId: z.guid().nonempty().nonoptional(),
    password: z.string().nonempty().nonoptional(),
  });
