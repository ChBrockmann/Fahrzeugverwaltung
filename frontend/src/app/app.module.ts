import {APP_INITIALIZER, ErrorHandler, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {ReservationCalendarComponent} from './reservation-calendar/reservation-calendar.component';
import {FullCalendarModule} from "@fullcalendar/angular";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {ApiModule, Configuration, ConfigurationParameters} from "./api";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, NG_VALIDATORS, ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MatMomentDateModule,
  MomentDateAdapter
} from "@angular/material-moment-adapter";
import {CreateReservationDialogComponent} from './create-reservation-dialog/create-reservation-dialog.component';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatLineModule} from "@angular/material/core";
import {MatSelectModule} from "@angular/material/select";
import {
  ViewReservationDetailsDialogComponent
} from './view-reservation-details-dialog/view-reservation-details-dialog.component';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatIconModule} from "@angular/material/icon";
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatDividerModule} from '@angular/material/divider';
import {RouterModule} from "@angular/router";
import {MatInputModule} from "@angular/material/input";
import {isAuthorizedGuard} from "./authentication/is-authorized.guard";
import { SetStatusForReservationActiongroupComponent } from './set-status-for-reservation-actiongroup/set-status-for-reservation-actiongroup.component';
import { AcceptInvitationComponent } from './accept-invitation/accept-invitation.component';
import { NotFoundComponent } from './not-found/not-found.component';
import {MatCardModule} from "@angular/material/card";
import {MatListModule} from "@angular/material/list";
import { ViewInvitationsComponent } from './view-invitations/view-invitations.component';
import { CreateInvitationComponent } from './create-invitation/create-invitation.component';
import {KeycloakAngularModule, KeycloakService} from "keycloak-angular";
import { MailSettingsComponent } from './mail-settings/mail-settings.component';
import {environment} from "../environments/environment";
import { DebugComponent } from './debug/debug.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {GlobalErrorHandler} from "./services/global-error-handler/global-error-handler.service";


export const MY_FORMATS = {
  parse: {
    dateInput: 'DD.MM.YYYY',
  },
  display: {
    dateInput: 'DD.MM.YYYY',
    monthYearLabel: 'DD.MM.YYYY',
    dateA11yLabel: 'DD.MM.YYYY',
    monthYearA11yLabel: 'DD.MM.YYYY',
  },
};

/*
    Why is the basepath set to an empty string?
    The basepath is used to prefix all api calls. If we set it to localhost:5000 then later in production that value cannot be changed.
    Instead, we are providing a relative baseUrl and let the proxy handle the rest.
 */
export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: "",
  };
  return new Configuration(params);
}

function initializeKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: environment.keycloak.url,
        realm: environment.keycloak.realm,
        clientId: environment.keycloak.clientId
      },
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri:
          window.location.origin + '/assets/silent-check-sso.html'
      },
      shouldAddToken: (request) => {
        const { method, url } = request;

        const isGetRequest = 'GET' === method.toUpperCase();
        const acceptablePaths = ['/assets', '/clients/public'];
        const isAcceptablePathMatch = acceptablePaths.some((path) =>
          url.includes(path)
        );

        return !(isGetRequest && isAcceptablePathMatch);
      }
    });
}

@NgModule({
  declarations: [
    AppComponent,
    ReservationCalendarComponent,
    CreateReservationDialogComponent,
    ViewReservationDetailsDialogComponent,
    SetStatusForReservationActiongroupComponent,
    AcceptInvitationComponent,
    NotFoundComponent,
    ViewInvitationsComponent,
    CreateInvitationComponent,
    MailSettingsComponent,
    DebugComponent,

  ],
  imports: [
    KeycloakAngularModule,
    ApiModule.forRoot(apiConfigFactory),
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FullCalendarModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDialogModule,
    FormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatDividerModule,
    MatInputModule,
    MatCardModule,
    MatListModule,
    MatLineModule,
    MatCheckboxModule,
    MatAutocompleteModule,
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService]
    },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: MY_FORMATS
    },
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
