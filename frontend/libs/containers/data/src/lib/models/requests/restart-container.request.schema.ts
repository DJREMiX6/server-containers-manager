import { z } from 'zod';
import { RestartContainerRequest } from './restart-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const RestartContainerRequestSchema: z.ZodType<RestartContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
