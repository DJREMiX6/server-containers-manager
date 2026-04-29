import { TestBed } from '@angular/core/testing';

import { NamespacesService } from './namespaces.service';

describe('NamespacesService', () => {
  let service: NamespacesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NamespacesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
