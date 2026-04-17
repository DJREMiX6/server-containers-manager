import { NamespaceDto, UserRoleDto } from "@scm/auth/data";

export type UserDto = {
    id: string;
    username: string;
    roles: UserRoleDto[];
    namespaces: NamespaceDto[];
    isConfirmed: boolean;
    lastLoginDate: string | null;
}