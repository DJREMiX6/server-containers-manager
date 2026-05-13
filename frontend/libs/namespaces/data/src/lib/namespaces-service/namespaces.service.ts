import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
  CreateNamespaceResponse,
  CreateNamespaceResponseSchema,
  GetNamespacesResponse,
  GetNamespacesResponseSchema,
} from '../models';
import {
  CheckNameAvailabilityRequest,
  CheckNameAvailabilityRequestSchema,
  CreateNamespaceRequest,
  CreateNamespaceRequestSchema,
} from '../models/requests';

export const ApiBaseUrl = `${Environment.serverOrigin}/api/namespaces`;

@Injectable()
export class NamespacesService {
  private readonly httpClient = inject(HttpClient);

  public getNamespaces(): Observable<GetNamespacesResponse> {
    return this.httpClient
      .get<unknown>(ApiBaseUrl)
      .pipe(map((raw) => GetNamespacesResponseSchema.parse(raw)));
  }

  public createNamespace(
    request: CreateNamespaceRequest,
  ): Observable<CreateNamespaceResponse> {
    const parsedRequest = CreateNamespaceRequestSchema.parse(request);

    return this.httpClient
      .post<unknown>(ApiBaseUrl, {
        ...parsedRequest,
      })
      .pipe(map((raw) => CreateNamespaceResponseSchema.parse(raw)));
  }

  public checkNameAvailability(
    request: CheckNameAvailabilityRequest,
  ): Observable<HttpResponse<void>> {
    const parsedRequest = CheckNameAvailabilityRequestSchema.parse(request);

    return this.httpClient.head<void>(`${ApiBaseUrl}/check-name`, {
      params: {
        ...parsedRequest,
      },
      observe: 'response',
    });
  }
}
