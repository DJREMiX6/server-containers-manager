import { ContainerPortDto } from '@scm/containers/data';
import { ContainerPort } from '../models';

export function containerPortMapper(dto: ContainerPortDto): ContainerPort {
  return {
    public: dto.public,
    private: dto.private,
  };
}

export function containerPortsMapper(
  dtos: ContainerPortDto[],
): ContainerPort[] {
  return dtos.map(containerPortMapper);
}
