import {
  inject,
  makeEnvironmentProviders,
  provideAppInitializer,
} from '@angular/core';
import { AuthService } from '@scm/auth/data';
import { AuthStore } from '@scm/auth/state';
import { authInterceptor } from './auth-interceptor/auth-interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

export function provideAuthentication() {
  return makeEnvironmentProviders([
    AuthService,
    AuthStore,
    provideAuthInitializer(),
    provideAuthInterceptor(),
  ]);
}

function provideAuthInitializer() {
  return provideAppInitializer(() => {
    const authStore = inject(AuthStore);
    return authStore.checkAuth();
  });
}

function provideAuthInterceptor() {
  return makeEnvironmentProviders([
    {
      provide: HTTP_INTERCEPTORS,
      useExisting: authInterceptor,
      multi: true,
    },
  ]);
}
