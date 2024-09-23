import { Component } from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import {TestService} from "../api";

@Component({
  selector: 'app-debug',
  templateUrl: './debug.component.html',
  styleUrls: ['./debug.component.scss']
})
export class DebugComponent {

  constructor(private readonly keycloakService: KeycloakService,
              private readonly testEndpoint: TestService) {
  }

  profile() {
    this.keycloakService.loadUserProfile().then(profile => {
      console.log(profile);
    });
  }

  token() {
    this.keycloakService.getToken().then(token => {
      console.log(token);
    });
  }

  testEp() {
    this.testEndpoint.testEndpoint().subscribe(response => {
      console.log(response);
    });
  }
}
