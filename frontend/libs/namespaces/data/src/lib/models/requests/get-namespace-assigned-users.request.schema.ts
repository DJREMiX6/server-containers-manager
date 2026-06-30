import { z } from 'zod';
import { GetNamespaceAssignedUsersRequest } from './get-namespace-assigned-users.request';

export const GetNamespaceAssignedUsersRequestSchema: z.ZodType<GetNamespaceAssignedUsersRequest> =
  z.object({
    namespaceId: z.guid().nonempty().nonoptional(),
  });
