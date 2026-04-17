export { AuthService } from './lib/auth-service/auth.service';

export type {
  LoginRequest,
  ChangePasswordRequest,
  GetSessionInfoResponse,
  NamespaceDto,
  UserRoleDto,
  UserDto,
} from './lib/models';

export {
  UserDtoSchema,
  UserRoleDtoSchema,
  NamespaceDtoSchema,
} from './lib/models';
