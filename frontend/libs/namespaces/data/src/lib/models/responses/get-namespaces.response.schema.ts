import { z } from 'zod';
import { GetNamespacesResponse } from './get-namespaces.response';
import { NamespaceDtoSchema } from '../dtos';

export const GetNamespacesResponseSchema: z.ZodType<GetNamespacesResponse> =
  z.object({
    namespaces: z.array(NamespaceDtoSchema),
  });
