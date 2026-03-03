import { inject } from '@angular/core';
import { patchState, signalStore, withMethods, withState } from '@ngrx/signals';
import { AuthService } from '@scm/auth/data';
import { firstValueFrom } from 'rxjs';
import { initialState } from './auth.state';
import { LoginRequestModel } from '../models/requests/login-request-model';
import { toUserRole } from '../models/user-role';
import { Namespace } from '../models/namespace';
import { SessionInfo } from '../models/responses/session-info';
import { Router } from '@angular/router';

export const AuthStore = signalStore(
  withState(initialState),
  withMethods(
    (state, authService = inject(AuthService), router = inject(Router)) => {
      const getSessionInfo = async (): Promise<SessionInfo> => {
        try {
          const sessionInfoResponse = await firstValueFrom(
            authService.getSessionInfo(),
          );
          return {
            userId: sessionInfoResponse.userId,
            username: sessionInfoResponse.username,
            roles: sessionInfoResponse.roles.map(toUserRole),
            namespaces: sessionInfoResponse.namespaces.map(
              (n): Namespace => ({
                id: n.id,
                name: n.name,
              }),
            ),
          };
        } catch (error) {
          patchState(state, { error: error as Error });
          throw error;
        }
      };

      const login = async (loginRequest: LoginRequestModel): Promise<void> => {
        try {
          await firstValueFrom(authService.login(loginRequest));
          patchState(state, { isAuthenticated: true });

          const sessionInfo = await getSessionInfo();
          patchState(state, {
            user: {
              id: sessionInfo.userId,
              username: sessionInfo.username,
              roles: sessionInfo.roles,
              namespaces: sessionInfo.namespaces,
            },
          });
        } catch (error) {
          patchState(state, { error: error as Error });
          throw error;
        }
      };

      const checkAuth = async () => {
        try {
          const sessionInfo = await getSessionInfo();

          patchState(state, {
            isAuthenticated: true,
            user: {
              id: sessionInfo.userId,
              username: sessionInfo.username,
              roles: sessionInfo.roles,
              namespaces: sessionInfo.namespaces,
            },
            error: null,
          });
        } catch {
          router.navigate(['auth', 'login']);
        }
      };

      return { login, checkAuth };
    },
  ),
);
