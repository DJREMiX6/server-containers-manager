import { NamespaceInfo } from '@scm/auth/data';
import { Namespace } from '../models/domain/namespace';

export function namespaceInfoDtoMapper(dto: NamespaceInfo): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}
