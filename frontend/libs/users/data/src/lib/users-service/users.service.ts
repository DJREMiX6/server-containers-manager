import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Environment } from '@scm/environments';
import { GetUsersResponse, GetUsersResponseSchema } from '../models';

export const ApiBaseEndpoint = `${Environment.serverOrigin}/api/users`;

@Injectable()
export class UsersService {
  private readonly httClient = inject(HttpClient);

  public getUsers(): Observable<GetUsersResponse> {
    return this.httClient
      .get<unknown>(ApiBaseEndpoint)
      .pipe(map((raw) => GetUsersResponseSchema.parse(raw)));
  }
}
