import { z } from 'zod';
import { GetSessionInfoResponse } from './get-session-info.response';
import { UserDtoSchema } from '../dto';

export const GetSessionInfoSchema: z.ZodType<GetSessionInfoResponse> = z.object(
  {
    user: UserDtoSchema,
  },
);
