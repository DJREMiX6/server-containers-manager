import { NamespaceInfo } from '../dto';

export type GetSessionInfoResponse = {
  userId: string;
  username: string;
  roles: string[];
  namespaces: NamespaceInfo[];
};
