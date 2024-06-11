import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "./services/authentication/authentication.service";
import {IdentityService} from "./api";
import {environment} from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Fahrzeugverwaltung';

  routes: { path: string; allowedRoles: string[]; icon: string; title: string }[] = [
    {
      path: "/",
      allowedRoles: [],
      icon: "home",
      title: "Startseite"
    },
    {
      path: "invitations",
      allowedRoles: [environment.roles.admin],
      icon: "mark_email_unread",
      title: "Einladungen"
    },
  ]

  constructor(private readonly router: Router,
              private readonly authService: AuthenticationService,
              private readonly logoutService: IdentityService) {
  }

  logout(): void {
    this.logoutService.logoutEndpoint().subscribe();
    this.authService.clear();
    this.router.navigate(["login"]);
  }

  showMenu(): boolean {
    return this.authService.hasUser() &&
      (this.authService.getUser()?.roles?.includes(environment.roles.admin) ?? false);
  }

  getMenuItems(): { path: string, icon: string, title: string }[] {
    return this.routes.filter(route => {
      if(route.allowedRoles.length == 0) {
        return true;
      }

      let userRoles = this.authService.getUser()?.roles ?? [];

      return route.allowedRoles.some(role => userRoles.includes(role));
    });
  }

}
