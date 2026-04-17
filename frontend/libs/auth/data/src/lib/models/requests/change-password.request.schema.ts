import { z } from 'zod';
import { ChangePasswordRequest } from './change-password.request';

export const ChangePasswordRequestSchema: z.ZodType<ChangePasswordRequest> =
  z.object({
    currentPassword: z.string().nonempty().nonoptional(),
    newPassword: z.string().nonempty().nonoptional(),
  });
