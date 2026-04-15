import { z } from 'zod';
import { StartContainerRequest } from './start-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const StartContainerRequestSchema: z.ZodType<StartContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
