import * as z from 'zod';
import { ContainersSortByDto } from './containers-sort-by.dto';

export const ContainersSortByDtoSchema: z.ZodType<ContainersSortByDto> = z.literal([
  'name',
  'status',
  'created',
]);
