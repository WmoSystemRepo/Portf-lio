import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../../features/anypoint/auth';
import { environment } from '../../../environments/environment';

export const AuthInterceptorFn: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const http = inject(HttpClient);

  const token = authService.getToken();

  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const refreshToken = authService.getRefreshToken();

        if (!refreshToken) {
          authService.logout();
          window.location.href = '/auth';
          return throwError(() => new Error('No refresh token'));
        }

        return http
          .post<any>(`${environment.api}/account/refresh-token`, {
            refreshToken: refreshToken,
          })
          .pipe(
            switchMap((response) => {
                localStorage.setItem('auth', response.token);
              localStorage.setItem('refresh_token', response.refreshToken);

              const newReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${response.token}`,
                },
              });

              return next(newReq);
            }),
            catchError((err) => {
              authService.logout();
              window.location.href = '/auth';
              return throwError(() => err);
            })
          );
      } else {
        return throwError(() => error);
      }
    })
  );
};
