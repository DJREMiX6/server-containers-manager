import { NamespaceDto } from './namespace.dto';
import { UserRoleDto } from './user-role.dto';

export type UserDto = {
  userId: string;
  username: string;
  roles: UserRoleDto[];
  namespaces: NamespaceDto[];
  isConfirmed: boolean;
};
