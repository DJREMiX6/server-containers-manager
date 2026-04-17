import { NamespaceDtoSchema, UserRoleDtoSchema } from '@scm/auth/data';
import { UserDto } from './user.dto';
import { z } from 'zod';

export const UserDtoSchema: z.ZodType<UserDto> = z.object({
  id: z.guid().nonempty().nonoptional(),
  username: z.string().nonempty().nonoptional(),
  roles: z.array(UserRoleDtoSchema),
  namespaces: z.array(NamespaceDtoSchema),
  isConfirmed: z.boolean().nonoptional(),
  lastLoginDate: z.iso.datetime().nonempty().nonoptional().nullable(),
});
