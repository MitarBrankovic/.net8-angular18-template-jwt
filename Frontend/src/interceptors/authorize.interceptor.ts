import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, map, of, switchMap, throwError } from 'rxjs';
import { UserService } from 'src/app/services/user.service';

@Injectable()
export class HttpInterceptorService implements HttpInterceptor {
  constructor(private router: Router, private userService: UserService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('Template_jwt');

    if (token) {
      request = this.addToken(request, token);
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(request, next);
        }
        return throwError(error);
      })
    );
  }
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let dataToSend = {
      accessToken: localStorage.getItem('Template_jwt'),
      refreshToken: localStorage.getItem('Template_refreshToken'),
    };
    if (!dataToSend.accessToken || !dataToSend.refreshToken) {
      this.router.navigate(['/']);
      return throwError(
        () => new Error('Tokens are missing, redirecting to login page.')
      );
    }
    return this.userService.getRefreshToken(dataToSend).pipe(
      switchMap((response: any) => {
        const newToken = response.accessToken;
        const newRefreshToken = response.refreshToken;

        if (newToken && newRefreshToken) {
          localStorage.setItem('Template_jwt', newToken);
          localStorage.setItem('Template_refreshToken', newRefreshToken);
          const newRequest = this.addToken(request, newToken);
          return next.handle(newRequest);
        }
        this.router.navigate(['/']);
        return throwError(() => new Error('Failed to refresh token'));
      }),
      catchError((err) => {
        this.router.navigate(['/']);
        return of(null as unknown as HttpEvent<any>);
      })
    );
  }
}
