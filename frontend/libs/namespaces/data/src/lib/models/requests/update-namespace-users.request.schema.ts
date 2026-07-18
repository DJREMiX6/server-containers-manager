import { z } from 'zod';
import {
  UpdateNamespaceUsersRequest,
  UpdateNamespaceUsersRequestData,
} from './update-namespace-users.request';

export const UpdateNamespaceUsersRequestDataSchema: z.ZodType<UpdateNamespaceUsersRequestData> =
  z.object({
    associatedUserIds: z
      .array(z.string().nonempty().nonoptional())
      .nonoptional(),
  });

export const UpdateNamespaceUsersRequestSchema: z.ZodType<UpdateNamespaceUsersRequest> =
  z.object({
    namespaceId: z.guid().nonempty().nonoptional(),
    data: UpdateNamespaceUsersRequestDataSchema,
  });
