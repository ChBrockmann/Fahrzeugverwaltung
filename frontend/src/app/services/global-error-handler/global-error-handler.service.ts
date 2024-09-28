import {ErrorHandler, Injectable, Injector} from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private readonly keycloakService: KeycloakService) {
  }

  handleError(error: HttpErrorResponse): void {
    console.log(error);
    if (error.status === 401) {
      console.log("401 error");
    }
    if (error instanceof HttpErrorResponse) {
      console.log("HttpErrorResponse");
    }
  }
}
