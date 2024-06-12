import {CanActivate, CanActivateFn, Router} from '@angular/router';
import {inject, Injectable} from "@angular/core";
import {AuthenticationService} from "../services/authentication/authentication.service";



export const isAuthorizedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthenticationService);
  let router = inject(Router);
  if (authService.hasUser())
  {
    return true;
  }
  else
  {
    router.navigate(["login"]).then(r => console.log("Navigated to login"));
    return false;
  }
};

export const isUnAuthorizedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthenticationService);
  let router = inject(Router);
  if (authService.hasUser())
  {
    router.navigate(["/"]).then(r => console.log("Navigated to /"));
    return false;
  }
  else
  {
    return true;
  }
};

export const hasRoleGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthenticationService);
  let requiredRoles: string[] = route.data['roles'];
  let userRoles = authService.getUser()?.roles ?? [];

  return requiredRoles.some(role => userRoles.includes(role));
};
