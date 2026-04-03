import {
  inject,
  makeEnvironmentProviders,
  provideAppInitializer,
} from '@angular/core';
import { AuthService } from '@scm/auth/data';
import { AuthStore } from '@scm/auth/store';
import { authInterceptor } from './auth-interceptor/auth-interceptor';
import { HttpInterceptorFn } from '@angular/common/http';
import { credentialsInterceptor } from './credentials-interceptor/credentials-interceptor';

export function provideAuthentication() {
  return makeEnvironmentProviders([
    AuthService,
    AuthStore,
    provideAuthInitializer(),
  ]);
}

export function withAuthInterceptors(): HttpInterceptorFn[] {
  return [credentialsInterceptor, authInterceptor];
}

function provideAuthInitializer() {
  return provideAppInitializer(() => {
    const authStore = inject(AuthStore);
    return authStore.checkAuth();
  });
}
