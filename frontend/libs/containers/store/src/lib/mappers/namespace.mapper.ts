import { NamespaceDto } from '@scm/containers/data';
import { Namespace } from '../models';

export function namespaceMapper(dto: NamespaceDto): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}

export function namespacesMapper(dtos: NamespaceDto[]): Namespace[] {
    return dtos.map(namespaceMapper);
}
