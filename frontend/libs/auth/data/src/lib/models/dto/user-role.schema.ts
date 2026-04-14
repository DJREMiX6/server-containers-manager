import { z } from 'zod';
import { UserRoleDto } from './user-role.dto';

export const UserRoleSchema: z.ZodType<UserRoleDto> = z.literal([
  'Admin',
  'Member',
]);
