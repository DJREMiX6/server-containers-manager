import { inject } from '@angular/core';
import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { setError, clearError } from '@scm/shared/store/error-store-feature';
import { AuthService } from '@scm/auth/data';
import { firstValueFrom } from 'rxjs';
import { withAuthState } from './auth.state';
import { LoginRequestModel } from '../models/requests/login-request-model';
import { toUserRole } from '../models/user-role';
import { Namespace } from '../models/namespace';
import { SessionInfo } from '../models/responses/session-info';
import { Router } from '@angular/router';

export const AuthStore = signalStore(
  withAuthState(),
  withMethods(
    (store, authService = inject(AuthService), router = inject(Router)) => {
      const getSessionInfo = async (): Promise<SessionInfo> => {
        try {
          patchState(store, clearError());

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
          patchState(store, setError(error));
          throw error;
        }
      };

      const login = async (loginRequest: LoginRequestModel): Promise<void> => {
        try {
          patchState(store, clearError());

          await firstValueFrom(authService.login(loginRequest));
          const sessionInfo = await getSessionInfo();
          patchState(store, {
            isAuthenticated: true,
            user: {
              id: sessionInfo.userId,
              username: sessionInfo.username,
              roles: sessionInfo.roles,
              namespaces: sessionInfo.namespaces,
            },
          });
        } catch (error) {
          patchState(
            store,
            {
              isAuthenticated: false,
              user: null,
            },
            setError(error),
          );
          throw error;
        }
      };

      const logout = async () => {
        try {
          await firstValueFrom(authService.logout());
        } finally {
          patchState(store, {
            isAuthenticated: false,
            user: null,
          });
        }
      };

      const checkAuth = async () => {
        try {
          patchState(store, clearError());

          const sessionInfo = await getSessionInfo();

          patchState(store, {
            isAuthenticated: true,
            user: {
              id: sessionInfo.userId,
              username: sessionInfo.username,
              roles: sessionInfo.roles,
              namespaces: sessionInfo.namespaces,
            },
            error: null,
          });
        } catch (error) {
          patchState(
            store,
            {
              isAuthenticated: false,
              error: null,
              user: null,
            },
            setError(error),
          );
          router.navigate(['auth', 'login']);
        }
      };

      return { login, logout, checkAuth };
    },
  ),
);
