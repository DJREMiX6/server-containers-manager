import { z } from 'zod';
import { GetNamespaceAssociatedContainersRequest } from './get-namespace-associated-containers.request';

export const GetNamespaceAssociatedContainersRequestSchema: z.ZodType<GetNamespaceAssociatedContainersRequest> =
  z.object({
    namespaceId: z.guid().nonempty().nonoptional(),
  });
