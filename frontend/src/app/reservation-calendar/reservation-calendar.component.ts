import {Component, OnInit} from '@angular/core';
import {CalendarOptions, DateSelectArg, DatesSetArg, EventInput} from "fullcalendar";
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import {ReservationModelDto, ReservationService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment/moment";
import {MatDialog} from "@angular/material/dialog";
import {CreateReservationDialogComponent} from "../create-reservation-dialog/create-reservation-dialog.component";
import {
  ViewReservationDetailsDialogComponent
} from "../view-reservation-details-dialog/view-reservation-details-dialog.component";

interface loadedMonths {
  year: number,
  month: number,
}

@Component({
  selector: 'app-reservation-calendar',
  templateUrl: './reservation-calendar.component.html',
  styleUrls: ['./reservation-calendar.component.scss']
})
export class ReservationCalendarComponent {

  events: EventInput[] = [];
  loadedMonths: loadedMonths[] = [];

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    firstDay: 1,
    plugins: [dayGridPlugin, interactionPlugin],
    events: this.events,
    locale: 'de-DE',
    selectable: true,
    selectMirror: true,
    editable: false,
    buttonText: {
      today: 'Aktueller Monat'
    },
    datesSet: async (eventArgs) => this.dateChanged(eventArgs),
    select: (eventArgs) => this.createReservationWithSelevtEvent(eventArgs),
    eventClick: (eventArgs) => this.clickOnEvent(eventArgs)
  }

  constructor(private readonly reservationService: ReservationService,
              public dialog: MatDialog) {
    this.events = [];
  }

  async getEventsForMonthYear(startdate: moment.Moment, enddate: moment.Moment): Promise<EventInput[]> {
    if (this.loadedMonths.find(value => value.year === startdate.year() && value.month === startdate.month())) {
      return [];
    }
    let events = await firstValueFrom(
      this.reservationService.getReservationsInTimespanEndpoint(startdate.format("yyyy-M-D"), enddate.format("yyyy-M-D")));

    this.loadedMonths.push({
      year: startdate.year(),
      month: startdate.month()
    })

    return events.reservations.map(event => this.mapReservationModelToEvent(event));
  }

  mapReservationModelToEvent(input: ReservationModelDto): EventInput {
    return {
      title: this.getTitleForReservation(input),
      allDay: true,
      start: input.startDateInclusive,
      end: moment.utc(input.endDateInclusive).add(1, 'days').toISOString(),
      id: input.id,
      color: this.getColorForReservation(input)
    }
  }

  getColorForReservation(reservation: ReservationModelDto): string {
    switch (reservation.currentStatus) {
      case 'Pending':
        return '#A3A3A3'
        // return '#FFB833';
      case 'Confirmed':
        return '#58B455';
      case 'Denied':
        return '#CF2626'
    }
    return '';
  }

  getTitleForReservation(reservation: ReservationModelDto): string {

    let fahrzeug = reservation.vehicleReserved?.name ?? "Fahrzeug";
    let organization = reservation.reservationMadeByUser?.organization?.name ?? "Organisation";
    let fullname = reservation.reservationMadeByUser?.fullname ?? "Unbekannter Nutzer";

    switch (reservation.currentStatus) {
      case 'Confirmed':
        return `${fahrzeug} reserviert für ${organization} von ${fullname}`
      case 'Pending':
      default:
        return `${fahrzeug} angefragt für ${organization} von ${fullname}`
      case 'Denied':
        return `${fahrzeug} Anfrage abgelehnt`
    }
  }

  async dateChanged(eventArgs: DatesSetArg): Promise<void> {
    let startdate = moment.utc(eventArgs.start);
    let enddate = moment.utc(eventArgs.end);

    let monthYearEvents = await this.getEventsForMonthYear(startdate, enddate);

    for (let event of monthYearEvents) {
      if (!this.events.find(value => value.id === event.id)) {
        this.events.push(event);
      }
    }

    this.calendarOptions.events = this.events;
  }

  clickOnEvent(eventArgs: any): void {
    const dialog = this.dialog.open(ViewReservationDetailsDialogComponent, {
      data: {reservationId: eventArgs.event.id}
    });

    dialog.afterClosed().subscribe(result => {
      if(result == undefined || result == null){
        return;
      }
      if (result.wasDeleted !== undefined && result.wasDeleted) {
        this.events = this.events.filter(value => value.id !== result.reservationId);
        this.calendarOptions.events = this.events;
      }
      if(result.wasChanged !== undefined && result.wasChanged) {
        this.reloadSingleReservation(result.reservationId).then(r => {});
      }
    });
  }

  async reloadSingleReservation(reservationId: string) {
    this.events = this.events.filter(value => value.id !== reservationId);

    let result = await firstValueFrom(this.reservationService.getReservationByIdEndpoint(reservationId));

    let mapped = this.mapReservationModelToEvent(result.reservation);

    this.events.push(mapped);
    this.calendarOptions.events = this.events;
  }

  createReservation(): void {
    this.openDialog(undefined);
  }

  createReservationWithSelevtEvent(eventArgs: DateSelectArg): void {
    this.openDialog({startDate: moment.utc(eventArgs.start).add(1, 'day'), endDate: moment.utc(eventArgs.end)});
  }

  openDialog(data: { startDate: moment.Moment, endDate: moment.Moment } | undefined): void {
    const dialog = this.dialog.open(CreateReservationDialogComponent, {
      autoFocus: false,
      data: data
    });

    dialog.afterClosed().subscribe(result => {
      if (result == null) {
        return;
      }
      let mapped = this.mapReservationModelToEvent(result);

      this.events.push(mapped);
      this.calendarOptions.events = this.events;
    });
  }


}
