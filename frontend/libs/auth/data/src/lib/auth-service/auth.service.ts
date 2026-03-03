import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Environment } from "@scm/environments"; 
import { LoginRequest } from "../models/requests";
import { GetSessionInfoResponse } from '../models/responses';

export const ApiBaseEndpoint = `${Environment.serverOrigin}/api/auth`;

@Injectable()
export class AuthService {
  private readonly httpClient = inject(HttpClient);

  public login(request: LoginRequest): Observable<void> {
    return this.httpClient.post<void>(`${ApiBaseEndpoint}/signin`, request, {
      withCredentials: true,
    });
  }

  public getSessionInfo(): Observable<GetSessionInfoResponse> {
    return this.httpClient.get<GetSessionInfoResponse>(
      `${ApiBaseEndpoint}/session`,
      { withCredentials: true },
    );
  }
}