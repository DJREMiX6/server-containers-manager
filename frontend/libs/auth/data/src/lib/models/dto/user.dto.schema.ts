import { z } from 'zod';
import { UserDto } from './user.dto';
import { UserRoleDtoSchema } from './user-role.schema';
import { NamespaceDtoSchema } from './namespace.dto.schema';

export const UserDtoSchema: z.ZodType<UserDto> = z.object({
  userId: z.guid().nonempty(),
  username: z.string().nonempty(),
  roles: z.array(UserRoleDtoSchema).nonempty(),
  namespaces: z.array(NamespaceDtoSchema),
  isConfirmed: z.boolean().nonoptional(),
});
