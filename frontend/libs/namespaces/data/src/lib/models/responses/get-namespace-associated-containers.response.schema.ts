import { z } from 'zod';
import { GetNamespaceAssociatedContainersResponse } from './get-namespace-associated-containers.response';
import { NamespaceAssociatedContainerDtoSchema } from '../dtos';

export const GetNamespaceAssociatedContainersResponseSchema: z.ZodType<GetNamespaceAssociatedContainersResponse> =
  z.object({
    associatedContainers: z.array(NamespaceAssociatedContainerDtoSchema),
  });
