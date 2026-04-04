import { NamespaceInfo } from "./namespace-info.dto";
import { UserRoleDto } from "./user-role.dto";

export type UserDto = {
  userId: string;
  username: string;
  roles: UserRoleDto[];
  namespaces: NamespaceInfo[];
  isConfirmed: boolean;
};
