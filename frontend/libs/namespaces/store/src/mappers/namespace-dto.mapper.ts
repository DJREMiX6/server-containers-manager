import { NamespaceDto } from '@scm/namespaces/data';
import { Namespace } from '../models';

export function namespaceDtoMapper(dto: NamespaceDto): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}
