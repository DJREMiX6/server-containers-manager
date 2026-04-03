import * as z from 'zod';
import { KillContainerRequest } from './kill-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const KillContainerRequestSchema: z.ZodType<KillContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
