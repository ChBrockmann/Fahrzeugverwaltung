import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {TestService} from "./api";
import {environment} from "../environments/environment";
import {KeycloakService} from "keycloak-angular";
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Fahrzeugverwaltung';

  routes: { path: string; allowedRoles: string[]; icon: string; title: string; action?: () => void }[] = [
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
    {
      path: "mail-settings",
      allowedRoles: [environment.roles.admin, environment.roles.organizationAdmin],
      icon: "email",
      title: "Mail Einstellungen"
    },



    {
      path: "",
      allowedRoles: [],
      icon: "account_circle",
      title: "Profil",
      action: () => this.navigateToProfile()
    }
  ]

  constructor(private readonly titleService: Title,
              private readonly keycloakService: KeycloakService,
              ) {
    titleService.setTitle("Fahrzeugverwaltung");
  }

  async logout(): Promise<void> {
    this.keycloakService.clearToken();
    await this.keycloakService.logout();
  }

  async navigateToProfile() {
    await this.keycloakService.getKeycloakInstance().accountManagement()
  }

  getMenuItems(): { path: string, icon: string, title: string, action?: () => void }[] {
    let userRoles = this.keycloakService.getUserRoles() ?? [];
    return this.routes.filter(route => {
      if(route.allowedRoles.length == 0) {
        return true;
      }

      return route.allowedRoles.some(role => userRoles.includes(role));
    });
  }

}
