import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {TestService} from "./api";
import {environment} from "../environments/environment";
import {KeycloakService} from "keycloak-angular";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Fahrzeugverwaltung';

  routes: { path: string; allowedRoles: string[]; icon: string; title: string }[] = [
    {
      path: "calendar",
      allowedRoles: [],
      icon: "home",
      title: "Startseite"
    },
    {
      path: "invitations",
      allowedRoles: [environment.roles.admin, environment.roles.organizationAdmin],
      icon: "mark_email_unread",
      title: "Einladungen"
    },
  ]

  constructor(private readonly router: Router,
              private readonly testEndpoint: TestService,
              private readonly keycloakService: KeycloakService,
              ) {
  }

  async logout(): Promise<void> {
    await this.keycloakService.logout();
  }

  getMenuItems(): { path: string, icon: string, title: string }[] {
    let userRoles = this.keycloakService.getUserRoles() ?? [];
    return this.routes.filter(route => {
      if(route.allowedRoles.length == 0) {
        return true;
      }

      return route.allowedRoles.some(role => userRoles.includes(role));
    });
  }

}
