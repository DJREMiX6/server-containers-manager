import { z } from 'zod';
import { CreateUserResponse } from './create-user.response';

export const CreateUserResponseSchema: z.ZodType<CreateUserResponse> = z.object(
  {
    userId: z.guid().nonempty().nonoptional(),
  },
);
