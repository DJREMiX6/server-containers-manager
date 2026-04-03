import * as z from 'zod';
import { ResumeContainerRequest } from './resume-container.request';
import { ContainerIdSchema } from '../shared-schemas';

export const ResumeContainerRequestSchema: z.ZodType<ResumeContainerRequest> =
  z.object({
    containerId: ContainerIdSchema,
  });
