import { Injectable, inject } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HttpClient
} from '@angular/common/http';
import { Observable, throwError, switchMap, catchError } from 'rxjs';
import { AuthService } from '../../services/anypoint/auth/auth.service';
import { environment } from '../../../../environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    let authReq = req;
    if (token) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          const refreshToken = this.authService.getRefreshToken();

          if (!refreshToken) {
            this.authService.logout();
            window.location.href = '/auth';
            return throwError(() => new Error('No refresh token'));
          }

          return this.http.post<any>(`${environment.api}/account/refresh-token`, {
            refreshToken: refreshToken,
          }).pipe(
            switchMap((response) => {
              localStorage.setItem('auth', response.token);
              localStorage.setItem('refresh_token', response.refreshToken);

              const newReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${response.token}`,
                },
              });

              return next.handle(newReq);
            }),
            catchError((err) => {
              this.authService.logout();
              window.location.href = '/auth';
              return throwError(() => err);
            })
          );
        }

        return throwError(() => error);
      })
    );
  }
}
