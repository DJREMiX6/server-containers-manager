import { z } from 'zod';
import { CreateUserRequest } from './create-user.request';

export const CreateUserRequestSchema: z.ZodType<CreateUserRequest> = z.object({
  username: z.string().nonempty().nonoptional(),
  password: z.string().nonempty().nonoptional(),
});
