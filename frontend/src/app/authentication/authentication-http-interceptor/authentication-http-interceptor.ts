import {
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from "@angular/common/http";
import {Injectable, Provider} from "@angular/core";
import {catchError, Observable, of, throwError} from "rxjs";
import {Router} from "@angular/router";
import {KeycloakService} from "keycloak-angular";


@Injectable()
export class AuthenticationHttpInterceptor implements HttpInterceptor {

  //TODO CB 2024-09-20
  // constructor(private readonly keycloakService: KeycloakService) {
  // }
  //
  // private async handleAuthError(err: HttpErrorResponse): Promise<Observable<any>> {
  //
  //   if (err.status === 401 || err.status === 403) {
  //     await this.keycloakService.login();
  //
  //     return of();
  //   }
  //   return throwError(() => (err));
  // }
  //
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req);
    // return next.handle(req).pipe(catchError(x=> this.handleAuthError(x)));
  }

  // intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  //   let token = this.authService.getToken();
  //   if (token) {
  //     req = req.clone({setHeaders: {Authorization: `Bearer ${token}`}});
  //   }
  //   else {
  //
  //   }
  //   return next.handle(req);
  // }
}

export const authenticationHttpInterceptorProvider: Provider =
  {provide: HTTP_INTERCEPTORS, useClass: AuthenticationHttpInterceptor, multi: true};
