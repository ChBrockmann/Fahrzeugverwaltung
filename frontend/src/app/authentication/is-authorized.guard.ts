import {CanActivate, CanActivateFn, Router} from '@angular/router';
import {inject, Injectable} from "@angular/core";
import {AuthenticationService} from "../services/authentication/authentication.service";



export const isAuthorizedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthenticationService);
  let router = inject(Router);
  if (authService.hasToken())
  {
    return true;
  }
  else
  {
    router.navigate(["login"]);
    return false;
  }
};
