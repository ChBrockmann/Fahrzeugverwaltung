import { Component } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { TestService, UnauthorizedService } from '../api';
import { SignalRService } from '../services/signalr-service/signal-r.service';

@Component({
  selector: 'app-debug',
  templateUrl: './debug.component.html',
  styleUrls: ['./debug.component.scss'],
})
export class DebugComponent {
  constructor(
    private readonly keycloakService: KeycloakService,
    private readonly testEndpoint: TestService,
    private readonly unauthorizedService: UnauthorizedService,
    private readonly signalRService: SignalRService
  ) {}

  profile() {
    this.keycloakService.loadUserProfile().then((profile) => {
      console.log(profile);
    });
  }

  token() {
    this.keycloakService.getToken().then((token) => {
      console.log(token);
    });
  }

  testEp() {
    this.testEndpoint.testEndpoint().subscribe((response) => {
      console.log(response);
    });
  }

  unauthorized() {
    this.unauthorizedService.unAuthorizedEndpoint().subscribe((response) => {
      console.log(response);
    });
  }

  async signalr() {
    let token = await this.keycloakService.getToken();
    console.log(token);
    this.signalRService.startConnection().subscribe(() => {
      this.signalRService
        .receiveMessage('testsendmessage')
        .subscribe((message) => {
          console.log('Received message: ' + message);
        });
    });
  }
}
