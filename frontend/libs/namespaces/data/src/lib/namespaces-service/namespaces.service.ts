import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import { GetNamespacesResponse, GetNamespacesResponseSchema } from '../models';

export const ApiBaseUrl = `${Environment.serverOrigin}/api/namespaces`;

@Injectable()
export class NamespacesService {
  private readonly httpClient = inject(HttpClient);

  public getNamespaces(): Observable<GetNamespacesResponse> {
    return this.httpClient
      .get<unknown>(ApiBaseUrl)
      .pipe(map((raw) => GetNamespacesResponseSchema.parse(raw)));
  }
}
