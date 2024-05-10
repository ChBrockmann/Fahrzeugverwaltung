import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {ReservationCalendarComponent} from './reservation-calendar/reservation-calendar.component';
import {FullCalendarModule} from "@fullcalendar/angular";
import {HttpClientModule} from "@angular/common/http";
import {ApiModule} from "./api";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {
    MAT_MOMENT_DATE_ADAPTER_OPTIONS,
    MatMomentDateModule,
    MomentDateAdapter
} from "@angular/material-moment-adapter";
import { CreateReservationDialogComponent } from './create-reservation-dialog/create-reservation-dialog.component';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from "@angular/material/core";
import {MatSelectModule} from "@angular/material/select";


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

@NgModule({
    declarations: [
        AppComponent,
        ReservationCalendarComponent,
        CreateReservationDialogComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        FullCalendarModule,
        HttpClientModule,
        ApiModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatDialogModule,
        FormsModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatMomentDateModule,
        MatSelectModule,
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
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
