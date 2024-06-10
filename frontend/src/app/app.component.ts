import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "./services/authentication/authentication.service";
import {IdentityService} from "./api";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Fahrzeugverwaltung';

  constructor(private readonly router: Router,
              private readonly authService: AuthenticationService,
              private readonly logoutService: IdentityService) {
  }

  logout() : void {
    this.logoutService.logoutEndpoint().subscribe();
    this.authService.clear();
    this.router.navigate(["login"]);
  }
}
