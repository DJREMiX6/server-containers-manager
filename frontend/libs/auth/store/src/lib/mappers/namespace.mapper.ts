import { NamespaceInfo } from '@scm/auth/data';
import { Namespace } from '../models';

export function namespaceMapper(dto: NamespaceInfo): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}
