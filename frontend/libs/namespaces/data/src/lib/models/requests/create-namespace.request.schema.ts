import { z } from 'zod';
import { CreateNamespaceRequest } from './create-namespace.request';

export const CreateNamespaceRequestSchema: z.ZodType<CreateNamespaceRequest> =
  z.object({
    name: z.string().nonempty().nonoptional(),
  });
