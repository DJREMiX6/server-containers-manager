import * as z from 'zod';
import { NamespaceInfo } from './namespace-info.dto';

export const NamespaceInfoDtoSchema: z.ZodType<NamespaceInfo> = z.object({
  id: z.guid().nonempty(),
  name: z.string().nonempty(),
});
