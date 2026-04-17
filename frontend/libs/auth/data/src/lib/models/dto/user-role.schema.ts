import { z } from 'zod';
import { UserRoleDto } from './user-role.dto';

export const UserRoleDtoSchema: z.ZodType<UserRoleDto> = z.literal([
  'Admin',
  'Member',
]);
