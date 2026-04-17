import { NamespaceDto } from '@scm/auth/data';
import { Namespace } from '../models/domain/namespace';

export function namespaceInfoDtoMapper(dto: NamespaceDto): Namespace {
  return {
    id: dto.id,
    name: dto.name,
  };
}
