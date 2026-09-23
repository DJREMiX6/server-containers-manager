import { z } from 'zod';
import { UpdateUserNamespacesRequest } from './update-user-namespaces.request';

export const UpdateUserNamespacesRequestSchema: z.ZodType<UpdateUserNamespacesRequest> =
  z.object({
    userId: z.guid().nonempty().nonoptional(),
    namespacesIds: z.array(z.guid().nonempty().nonoptional()),
  });
