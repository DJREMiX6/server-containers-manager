import * as z from 'zod';
import { GetSessionInfoResponse } from './get-session-info.response';
import { NamespaceInfoDtoSchema, UserRoleSchema } from '../dto';

export const GetSessionInfoSchema: z.ZodType<GetSessionInfoResponse> = z.object(
  {
    userId: z.guid().nonempty(),
    username: z.string().nonempty(),
    roles: z.array(UserRoleSchema).nonempty(),
    namespaces: z.array(NamespaceInfoDtoSchema),
  },
);
