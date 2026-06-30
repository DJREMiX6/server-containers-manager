import { z } from 'zod';
import { NamespaceAssociatedUserDto } from './namespace-associated-user.dto';

export const NamespaceAssociatedUserDtoSchema: z.ZodType<NamespaceAssociatedUserDto> =
  z.object({
    id: z.guid().nonempty().nonoptional(),
    username: z.string().nonempty().nonoptional(),
  });
