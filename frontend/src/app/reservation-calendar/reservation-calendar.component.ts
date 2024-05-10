import {Component, OnInit} from '@angular/core';
import {CalendarOptions, DatesSetArg, EventInput} from "fullcalendar";
import dayGridPlugin from '@fullcalendar/daygrid';
import {ReservationModelDto, ReservationService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment/moment";
import {MatDialog} from "@angular/material/dialog";
import {CreateReservationDialogComponent} from "../create-reservation-dialog/create-reservation-dialog.component";

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
    locale: 'de-DE',
    datesSet: async (eventArgs) => this.dateChanged(eventArgs)
  }

  constructor(private readonly reservationService: ReservationService,
              public dialog: MatDialog) {
    this.events = [];
  }

  async getEventsForMonthYear(startdate: moment.Moment, enddate: moment.Moment): Promise<EventInput[]> {
    let events = await firstValueFrom(
        this.reservationService.getReservationsInTimespanEndpoint(startdate.format("yyyy-M-D"), enddate.format("yyyy-M-D")));

    return events.reservations.map(event => this.mapReservationModelToEvent(event));
  }

  mapReservationModelToEvent(input: ReservationModelDto) : EventInput {
    return {
      title: `Fahrzeug reserviert für ${input.reservationMadeByUser?.organization} von ${input.reservationMadeByUser?.fullname}`,
      allDay: true,
      start: input.startDateInclusive,
      end: moment.utc(input.endDateInclusive).add(1, 'days').toISOString(),
    }
  }

  async dateChanged(eventArgs: DatesSetArg) : Promise<void> {
    let startdate = moment.utc(eventArgs.start);
    let enddate = moment.utc(eventArgs.end);

    let monthYearEvents = await this.getEventsForMonthYear(startdate, enddate);

    for(let event of monthYearEvents) {
      this.events.push(event);
    }

    this.calendarOptions.events = this.events;
  }

  createReservation() : void {
    const dialog = this.dialog.open(CreateReservationDialogComponent, {autoFocus: false});

    dialog.afterClosed().subscribe(result => {
      let mapped = this.mapReservationModelToEvent(result);
      console.log(mapped);

      this.events.push(mapped);
      this.calendarOptions.events = this.events;
    });
  }


}
