import { NamespaceDto } from '@scm/auth/data';
import { Namespace } from '../models';

export function namespaceMapper(dto: NamespaceDto): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}
