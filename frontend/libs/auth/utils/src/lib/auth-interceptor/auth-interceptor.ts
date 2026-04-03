import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@scm/auth/data';
import { catchError, switchMap, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error) => {
      if (!(error instanceof HttpErrorResponse)) return throwError(() => error); // Forward if not HttpErrorResponse
      if (error.status !== HttpStatusCode.Unauthorized)
        return throwError(() => error); // Forward if error has not Unauthorized status code
      if (req.url.includes('/api/auth/session')) return throwError(() => error); // Forward if error comes from the session endpoint

      return authService.getSessionInfo().pipe(
        // If the API fails to return session info (user is not authenticated) redirect the user to login page
        catchError(() => {
          router.navigate(['auth', 'login']);
          return throwError(() => error);
        }),
        // If the API does not fail to return session info (user is authenticated) forward the error
        switchMap(() => throwError(() => error)),
      );
    }),
  );
};
