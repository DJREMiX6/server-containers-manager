import { User } from '../models';
import { userRoleMapper } from './user-role.mapper';
import { namespaceMapper } from './namespace.mapper';
import { GetSessionInfoResponse } from '@scm/auth/data';

export function userMapper(sessionInfo: GetSessionInfoResponse): User {
  return {
    id: sessionInfo.user.userId,
    username: sessionInfo.user.username,
    roles: sessionInfo.user.roles.map(userRoleMapper),
    namespaces: sessionInfo.user.namespaces.map(namespaceMapper),
    isConfirmed: sessionInfo.user.isConfirmed,
  };
}
