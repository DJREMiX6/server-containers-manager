import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import {
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
      {
        withCredentials: true,
      },
    );
  }

  public logout(): Observable<void> {
    return this.httpClient.post<void>(
      `${ApiBaseEndpoint}/signout`,
      {},
      {
        withCredentials: true,
      },
    );
  }

  public getSessionInfo(): Observable<GetSessionInfoResponse> {
    return this.httpClient
      .get<unknown>(`${ApiBaseEndpoint}/session`, { withCredentials: true })
      .pipe(map((raw) => GetSessionInfoSchema.parse(raw)));
  }
}