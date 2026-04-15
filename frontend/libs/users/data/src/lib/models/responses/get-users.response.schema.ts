import { z } from 'zod';
import { GetUsersResponse } from './get-users.response';
import { UserDtoSchema } from '@scm/auth/data';

export const GetUsersResponseSchema: z.ZodType<GetUsersResponse> = z.object({
  users: z.array(UserDtoSchema),
});
