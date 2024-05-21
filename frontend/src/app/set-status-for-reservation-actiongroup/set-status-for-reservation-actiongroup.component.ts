import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ReservationService, ReservationStatusEnum} from "../api";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-set-status-for-reservation-actiongroup',
  templateUrl: './set-status-for-reservation-actiongroup.component.html',
  styleUrls: ['./set-status-for-reservation-actiongroup.component.scss']
})
export class SetStatusForReservationActiongroupComponent {

  protected readonly ReservationStatusEnum = ReservationStatusEnum;

  @Input({ required: true }) reservationId!: string;
  protected reason: string | undefined;

  @Output() newStatus= new EventEmitter<ReservationStatusEnum>;

  constructor(private readonly reservationStatusService: ReservationService) {

  }

  async setStatusOfReservation(status: ReservationStatusEnum) {
    await firstValueFrom(this.reservationStatusService.createReservationStatusEndpoint(this.reservationId, {
      status: status,
      reason: this.reason,
    }));
    this.newStatus.emit(status);
  }


}
