import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
  ChangePasswordRequest,
  ChangePasswordRequestSchema,
  GetSessionInfoResponse,
  GetSessionInfoSchema,
  LoginRequest,
  LoginRequestSchema,
} from '../models';

export const ApiBaseEndpoint = `${Environment.serverOrigin}/api/auth`;

@Injectable()
export class AuthService {
  private readonly httpClient = inject(HttpClient);

  public login(request: LoginRequest): Observable<void> {
    const parsedRequest = LoginRequestSchema.parse(request);
    return this.httpClient.post<void>(
      `${ApiBaseEndpoint}/signin`,
      parsedRequest,
    );
  }

  public logout(): Observable<void> {
    return this.httpClient.post<void>(`${ApiBaseEndpoint}/signout`, null);
  }

  public getSessionInfo(): Observable<GetSessionInfoResponse> {
    return this.httpClient
      .get<unknown>(`${ApiBaseEndpoint}/session`)
      .pipe(map((raw) => GetSessionInfoSchema.parse(raw)));
  }

  public changePassword(request: ChangePasswordRequest): Observable<void> {
    const parsedRequest = ChangePasswordRequestSchema.parse(request);
    return this.httpClient.post<void>(
      `${ApiBaseEndpoint}/user/change-password`,
      {
        ...parsedRequest,
      },
    );
  }
}
