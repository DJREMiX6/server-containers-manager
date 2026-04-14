import { inject } from '@angular/core';
import { patchState, signalStore, withMethods } from '@ngrx/signals';
import { setError, clearError } from '@scm/shared/store/error-store-feature';
import { AuthService } from '@scm/auth/data';
import { firstValueFrom } from 'rxjs';
import { withAuthState } from './auth.state';
import { LoginRequestModel } from '../models/requests/login-request-model';
import { Router } from '@angular/router';
import { userMapper } from '../mappers';
import { ChangePasswordRequestModel } from '../models';

export const AuthStore = signalStore(
  withAuthState(),
  withMethods(
    (store, authService = inject(AuthService), router = inject(Router)) => {
      const login = async (loginRequest: LoginRequestModel): Promise<void> => {
        try {
          patchState(store, { requestStatus: 'pending' }, clearError());

          await firstValueFrom(authService.login(loginRequest));
          const sessionInfo = await firstValueFrom(
            authService.getSessionInfo(),
          );
          const user = userMapper(sessionInfo);

          patchState(
            store,
            {
              requestStatus: 'fullfilled',
              isAuthenticated: true,
              user,
            },
            clearError(),
          );
        } catch (error) {
          patchState(
            store,
            {
              requestStatus: 'rejected',
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

      const changePassword = async (request: ChangePasswordRequestModel) => {
        try {
          patchState(store, { requestStatus: 'pending' }, clearError());

          await firstValueFrom(authService.changePassword(request));

          patchState(store, {
            requestStatus: 'fullfilled',
          });
        } catch (error) {
          patchState(
            store,
            {
              requestStatus: 'rejected',
            },
            setError(error),
          );
          throw error;
        }
      };

      return { login, logout, checkAuth, changePassword };
    },
  ),
);
