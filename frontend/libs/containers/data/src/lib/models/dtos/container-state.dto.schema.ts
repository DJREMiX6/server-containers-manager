import * as z from 'zod';
import { ContainerStateDto } from './container-state.dto';

export const ContainerStateDtoSchema: z.ZodType<ContainerStateDto> = z.literal([
  'Created',
  'Running',
  'Paused',
  'Restarting',
  'Exited',
  'Removing',
  'Dead',
]);
