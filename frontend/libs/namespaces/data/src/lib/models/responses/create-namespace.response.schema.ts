import { z } from 'zod';
import { CreateNamespaceResponse } from './create-namespace.response';

export const CreateNamespaceResponseSchema: z.ZodType<CreateNamespaceResponse> =
  z.object({
    namespaceId: z.guid().nonempty().nonoptional(),
  });
