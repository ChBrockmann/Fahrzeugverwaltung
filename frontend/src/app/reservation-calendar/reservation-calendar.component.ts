import {Component, OnInit} from '@angular/core';
import {CalendarOptions, DatesSetArg, EventInput} from "fullcalendar";
import dayGridPlugin from '@fullcalendar/daygrid';
import {ReservationService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment/moment";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-reservation-calendar',
  templateUrl: './reservation-calendar.component.html',
  styleUrls: ['./reservation-calendar.component.scss']
})
export class ReservationCalendarComponent{

  events: EventInput[] = [];



  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin],
    events: this.events,
    locale: 'de',
    datesSet: async (eventArgs) => this.dateChanged(eventArgs)
  }

  constructor(private readonly reservationService: ReservationService,
              public dialog: MatDialog) {
  }

  async getEventsForMonthYear(startdate: moment.Moment, enddate: moment.Moment): Promise<EventInput[]> {
    let events = await firstValueFrom(
        this.reservationService.getReservationsInTimespanEndpoint(startdate.format("yyyy-M-D"), enddate.format("yyyy-M-D")));

    return events.reservations.map(event => {
      return {
        title: `Fahrzeug reserviert für ${event.reservationMadeByUser?.organization} von ${event.reservationMadeByUser?.fullname}`,
        allDay: true,
        start: event.startDateInclusive,
        end: moment.utc(event.endDateInclusive).add(1, 'days').toISOString(),
      }
    })
  }

  async dateChanged(eventArgs: DatesSetArg) : Promise<void> {
    console.log(eventArgs);
    let startdate = moment.utc(eventArgs.start);
    let enddate = moment.utc(eventArgs.end);

    this.calendarOptions.events = await this.getEventsForMonthYear(startdate, enddate);
  }

  createReservation() : void {
    // const dialog = this.dialog.open(CreateReservationDialogComponent);
    //
    // dialog.afterClosed().subscribe(result => {
    //
    // });
  }


}
