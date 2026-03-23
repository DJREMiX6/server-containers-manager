import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { map, Observable } from "rxjs";
import { Environment } from "@scm/environments";
import { GetContainersRequest, GetContainersResponse } from '../models';
import { GetContainersRequestSchema } from '../models/requests/get-containers.request.schema';
import { GetContainersResponseSchema } from '../models/responses/get-containers.response.schema';

export const ApiBaseUrl = `${Environment.serverOrigin}/api/containers`;

@Injectable()
export class ContainersService {
  private readonly httpClient = inject(HttpClient);

  public getContainers(
    request: GetContainersRequest,
  ): Observable<GetContainersResponse> {
    const parsedRequest = GetContainersRequestSchema.parse(request);

    return this.httpClient
      .get<unknown>(ApiBaseUrl, {
        params: {
          ...parsedRequest,
        },
        withCredentials: true,
      })
      .pipe(map((raw) => GetContainersResponseSchema.parse(raw)));
  }
}
