import { GetUsersResponse } from '@scm/users/data';
import { User } from '../models';
import { namespaceInfoDtoMapper } from './namespace-info.dto.mapper';
import { userRoleMapper } from './user-role.dto.mapper';

export function getUsersResponseMapper(response: GetUsersResponse): User[] {
  return response.users.map(
    (u): User => ({
      id: u.id,
      username: u.username,
      isConfirmed: u.isConfirmed,
      namespaces: u.namespaces.map(namespaceInfoDtoMapper),
      roles: u.roles.map(userRoleMapper),
      lastLoginDate: u.lastLoginDate ? new Date(u.lastLoginDate) : null,
      isAdmin: u.roles.map(userRoleMapper).includes('Admin'),
    }),
  );
}
