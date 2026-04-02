import * as z from 'zod';
import { StartContainerRequest } from './start-container.request';

export const StartContainerRequestSchema: z.ZodType<StartContainerRequest> =
  z.object({
    containerId: z.string().length(64).nonempty().nonoptional(),
  });
