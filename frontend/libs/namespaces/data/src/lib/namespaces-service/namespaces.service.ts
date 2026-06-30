import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
  CreateNamespaceResponse,
  CreateNamespaceResponseSchema,
  GetNamespaceAssignedUsersResponse,
  GetNamespaceAssignedUsersResponseSchema,
  GetNamespacesResponse,
  GetNamespacesResponseSchema,
} from '../models';
import {
  CheckNameAvailabilityRequest,
  CheckNameAvailabilityRequestSchema,
  CreateNamespaceRequest,
  CreateNamespaceRequestSchema,
  GetNamespaceAssignedUsersRequest,
  UpdateNamespaceUsersRequest,
  UpdateNamespaceUsersRequestSchema,
} from '../models/requests';
import { GetNamespaceAssignedUsersRequestSchema } from '../models/requests/get-namespace-assigned-users.request.schema';

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

  public getNamespaceAssignedUsers(
    request: GetNamespaceAssignedUsersRequest,
  ): Observable<GetNamespaceAssignedUsersResponse> {
    const parsedRequest = GetNamespaceAssignedUsersRequestSchema.parse(request);

    return this.httpClient
      .get<unknown>(`${ApiBaseUrl}/${parsedRequest.namespaceId}/users`)
      .pipe(map((raw) => GetNamespaceAssignedUsersResponseSchema.parse(raw)));
  }

  public updateNamespaceUsers(
    request: UpdateNamespaceUsersRequest,
  ): Observable<HttpResponse<void>> {
    const parsedRequest = UpdateNamespaceUsersRequestSchema.parse(request);

    return this.httpClient.patch<void>(
      `${ApiBaseUrl}/${parsedRequest.namespaceId}/users`,
      {
        ...parsedRequest.data,
      },
      { observe: 'response' },
    );
  }
}
