import { z } from 'zod';
import { DeleteUserRequest } from './delete-user.request';

export const DeleteUserRequestSchema: z.ZodType<DeleteUserRequest> = z.object({
  userId: z.guid().nonempty().nonoptional(),
});
