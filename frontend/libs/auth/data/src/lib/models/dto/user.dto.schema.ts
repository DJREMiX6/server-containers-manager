import { z } from 'zod';
import { UserDto } from './user.dto';
import { UserRoleSchema } from './user-role.schema';
import { NamespaceInfoDtoSchema } from './namespace-info.schema';

export const UserDtoSchema: z.ZodType<UserDto> = z.object({
  userId: z.guid().nonempty(),
  username: z.string().nonempty(),
  roles: z.array(UserRoleSchema).nonempty(),
  namespaces: z.array(NamespaceInfoDtoSchema),
  isConfirmed: z.boolean().nonoptional(),
});
