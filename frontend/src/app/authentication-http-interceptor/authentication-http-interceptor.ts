import {HTTP_INTERCEPTORS, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Injectable, Provider} from "@angular/core";
import {Observable} from "rxjs";
import {AuthenticationService} from "../services/authentication/authentication.service";


@Injectable()
export class AuthenticationHttpInterceptor implements HttpInterceptor {

  constructor(private readonly authService: AuthenticationService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log("Intercepted request", req);
    let token = this.authService.getToken();
    if (token) {
      req = req.clone({setHeaders: {Authorization: `Bearer ${token}`}});
    }
    else {
      throw new Error("No token found in local storage");
    }
    return next.handle(req);
  }
}

export const authenticationHttpInterceptorProvider: Provider =
  {provide: HTTP_INTERCEPTORS, useClass: AuthenticationHttpInterceptor, multi: true};
