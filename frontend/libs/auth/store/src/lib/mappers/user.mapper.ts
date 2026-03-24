import { User } from '../models';
import { userRoleMapper } from './user-role.mapper';
import { namespaceMapper } from './namespace.mapper';
import { GetSessionInfoResponse } from '@scm/auth/data';

export function userMapper(sessionInfo: GetSessionInfoResponse): User {
  return {
    id: sessionInfo.userId,
    username: sessionInfo.username,
    roles: sessionInfo.roles.map(userRoleMapper),
    namespaces: sessionInfo.namespaces.map(namespaceMapper),
  };
}
