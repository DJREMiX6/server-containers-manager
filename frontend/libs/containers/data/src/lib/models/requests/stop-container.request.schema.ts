import * as z from 'zod';
import { StopContainerRequest } from './stop-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const StopContainerRequestSchema: z.ZodType<StopContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
