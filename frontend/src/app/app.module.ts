import {NgModule} from '@angular/core';
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
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from "@angular/material/core";
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
import { LoginComponent } from './login/login.component';
import {RouterModule} from "@angular/router";
import {MatInputModule} from "@angular/material/input";
import {AuthenticationHttpInterceptor} from "./authentication/authentication-http-interceptor/authentication-http-interceptor";
import {isAuthorizedGuard} from "./authentication/is-authorized.guard";
import { SetStatusForReservationActiongroupComponent } from './set-status-for-reservation-actiongroup/set-status-for-reservation-actiongroup.component';


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

@NgModule({
  declarations: [
    AppComponent,
    ReservationCalendarComponent,
    CreateReservationDialogComponent,
    ViewReservationDetailsDialogComponent,
    LoginComponent,
    SetStatusForReservationActiongroupComponent,

  ],
  imports: [
    ApiModule.forRoot(apiConfigFactory),
    RouterModule.forRoot([
      {path: '', redirectTo: 'calendar', pathMatch: 'full'},
      {path: 'login', component: LoginComponent},
      {path: 'calendar', component: ReservationCalendarComponent, canActivate: [isAuthorizedGuard]},
    ]),
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
  ],
  providers: [
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
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationHttpInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
