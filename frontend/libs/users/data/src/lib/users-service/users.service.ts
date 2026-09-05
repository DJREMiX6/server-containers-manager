import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
  CheckUsernameAvailabilityRequest,
  CheckUsernameAvailabilityRequestSchema,
  CreateUserRequest,
  CreateUserRequestSchema,
  CreateUserResponse,
  CreateUserResponseSchema,
  GetUsersResponse,
  GetUsersResponseSchema,
} from '../models';

export const ApiBaseEndpoint = `${Environment.serverOrigin}/users`;

@Injectable()
export class UsersService {
  private readonly httpClient = inject(HttpClient);

  public getUsers(): Observable<GetUsersResponse> {
    return this.httpClient
      .get<unknown>(ApiBaseEndpoint)
      .pipe(map((raw) => GetUsersResponseSchema.parse(raw)));
  }

  public createUser(
    request: CreateUserRequest,
  ): Observable<CreateUserResponse> {
    const parsedRequest = CreateUserRequestSchema.parse(request);

    return this.httpClient
      .post<unknown>(ApiBaseEndpoint, {
        ...parsedRequest,
      })
      .pipe(map((raw) => CreateUserResponseSchema.parse(raw)));
  }

  public checkUsernameAvailability(
    request: CheckUsernameAvailabilityRequest,
  ): Observable<HttpResponse<void>> {
    const parsedRequest = CheckUsernameAvailabilityRequestSchema.parse(request);

    return this.httpClient.head<void>(`${ApiBaseEndpoint}/check-username`, {
      params: {
        ...parsedRequest,
      },
      observe: 'response',
    });
  }
}
