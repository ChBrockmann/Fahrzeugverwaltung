import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "./services/authentication/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Fahrzeugverwaltung';

  constructor(private readonly router: Router,
              private readonly authService: AuthenticationService) {
  }

  logout() : void {
    this.authService.claer();
    this.router.navigate(["login"]);
  }
}
