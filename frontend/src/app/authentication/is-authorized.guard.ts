import {CanActivateFn} from '@angular/router';
import {inject} from "@angular/core";
import {KeycloakService} from "keycloak-angular";



export const isAuthorizedGuard: CanActivateFn = async (route, state) => {
  let keycloakService = inject(KeycloakService);

  if (await keycloakService.isLoggedIn())
  {
    return true;
  }
  else
  {
    console.log("user is not logged in");
    await keycloakService.login({redirectUri: window.location.origin + state.url});
    return false;
  }
};

export const hasRoleGuard: CanActivateFn = (route, state) => {
  let keycloakService = inject(KeycloakService);

  let requiredRoles: string[] = route.data['roles'];
  let userRoles = keycloakService.getUserRoles() ?? [];

  return requiredRoles.some(role => userRoles.includes(role));
};
