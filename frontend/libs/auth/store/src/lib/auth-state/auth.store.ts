import { inject } from '@angular/core';
import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { setError, clearError } from '@scm/shared/store/error-store-feature';
import { AuthService } from '@scm/auth/data';
import { firstValueFrom } from 'rxjs';
import { withAuthState } from './auth.state';
import { LoginRequestModel } from '../models/requests/login-request-model';
import { Router } from '@angular/router';
import { userMapper } from '../mappers';

export const AuthStore = signalStore(
  withAuthState(),
  withMethods(
    (store, authService = inject(AuthService), router = inject(Router)) => {
      const login = async (loginRequest: LoginRequestModel): Promise<void> => {
        try {
          patchState(store, clearError());

          await firstValueFrom(authService.login(loginRequest));
          const sessionInfo = await firstValueFrom(
            authService.getSessionInfo(),
          );
          const user = userMapper(sessionInfo);

          patchState(
            store,
            {
              isAuthenticated: true,
              user,
            },
            clearError(),
          );
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

          const sessionInfo = await firstValueFrom(
            authService.getSessionInfo(),
          );
          const user = userMapper(sessionInfo);

          patchState(
            store,
            {
              isAuthenticated: true,
              user,
            },
            clearError(),
          );
        } catch (error) {
          patchState(
            store,
            {
              isAuthenticated: false,
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
