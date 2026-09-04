import { z } from 'zod';
import { NamespaceAssociatedContainerDto } from './namespace-associated-container.dto';

export const NamespaceAssociatedContainerDtoSchema: z.ZodType<NamespaceAssociatedContainerDto> =
  z.object({
    id: z.string().length(64).nonempty().nonoptional(),
    name: z.string().nonempty().nonoptional(),
  });
