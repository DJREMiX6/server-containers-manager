import { z } from 'zod';
import { GetNamespaceAssignedUsersResponse } from './get-namespace-assigned-users.response';
import { NamespaceAssociatedUserDtoSchema } from '../dtos';

export const GetNamespaceAssignedUsersResponseSchema: z.ZodType<GetNamespaceAssignedUsersResponse> =
  z.object({
    associatedUsers: z.array(NamespaceAssociatedUserDtoSchema),
  });
