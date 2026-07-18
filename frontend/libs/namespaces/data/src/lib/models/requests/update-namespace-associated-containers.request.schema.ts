import { z } from 'zod';
import {
  UpdateNamespaceAssociatedContainersRequest,
  UpdateNamespaceAssociatedContainersRequestData,
} from './update-namespace-associated-containers.request';

export const UpdateNamespaceAssociatedContainersRequestDataSchema: z.ZodType<UpdateNamespaceAssociatedContainersRequestData> =
  z.object({
    AssociatedContainersIds: z
      .array(z.string().nonempty().nonoptional())
      .nonoptional(),
  });

export const UpdateNamespaceAssociatedContainersRequestSchema: z.ZodType<UpdateNamespaceAssociatedContainersRequest> =
  z.object({
    namespaceId: z.guid().nonempty().nonoptional(),
    data: UpdateNamespaceAssociatedContainersRequestDataSchema,
  });
