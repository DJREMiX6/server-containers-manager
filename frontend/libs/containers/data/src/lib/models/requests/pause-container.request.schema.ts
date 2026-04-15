import { z } from 'zod';
import { PauseContainerRequest } from './pause-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const PauseContainerRequestSchema: z.ZodType<PauseContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
