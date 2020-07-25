import { tap } from 'rxjs/operators';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpResponse,
  HttpErrorResponse,
  HttpEvent,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

export class UnauthorizedInterceptor implements HttpInterceptor {

  constructor(public authService: AuthService,
              private router: Router) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
        tap(
            (event: HttpEvent<any>) => {
              if (event instanceof HttpResponse) {
                // do stuff with response if you want
              }
            },
            (err: any) => {
              if (err instanceof HttpErrorResponse) {
                if (err.status === 401) {
                  // redirect to the login route
                  // or show a modal
                  this.router.navigate(['']);
                }
              }
            }
          )
    );
  }
}
