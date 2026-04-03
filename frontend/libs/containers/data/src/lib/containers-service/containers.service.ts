import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
  GetContainersRequest,
  GetContainersResponse,
  GetContainersRequestSchema,
  GetContainersResponseSchema,
  StartContainerRequest,
  StartContainerRequestSchema,
  StopContainerRequest,
  StopContainerRequestSchema,
  PauseContainerRequest,
  PauseContainerRequestSchema,
  ResumeContainerRequest,
  ResumeContainerRequestSchema,
  RestartContainerRequest,
  RestartContainerRequestSchema,
  KillContainerRequest,
  KillContainerRequestSchema,
} from '../models';

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
      })
      .pipe(map((raw) => GetContainersResponseSchema.parse(raw)));
  }

  public startContainer(request: StartContainerRequest): Observable<void> {
    const parsedRequest = StartContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/start`,
      null,
    );
  }

  public stopContainer(request: StopContainerRequest): Observable<void> {
    const parsedRequest = StopContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/stop`,
      null,
    );
  }

  public pauseContainer(request: PauseContainerRequest): Observable<void> {
    const parsedRequest = PauseContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/pause`,
      null,
    );
  }

  public resumeContainer(request: ResumeContainerRequest): Observable<void> {
    const parsedRequest = ResumeContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/resume`,
      null,
    );
  }

  public restartContainer(request: RestartContainerRequest): Observable<void> {
    const parsedRequest = RestartContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/restart`,
      null,
    );
  }

  public killContainer(request: KillContainerRequest): Observable<void> {
    const parsedRequest = KillContainerRequestSchema.parse(request);

    return this.httpClient.post<void>(
      `${ApiBaseUrl}/${parsedRequest.containerId}/kill`,
      null,
    );
  }
}
