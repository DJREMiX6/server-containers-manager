import { Namespace } from '../namespace';
import { UserRole } from '../user-role';

export type SessionInfo = {
  userId: string;
  username: string;
  roles: UserRole[];
  namespaces: Namespace[];
};
