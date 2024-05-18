import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {GetReservationByIdResponse, ReservationModelDto, ReservationService} from "../api";
import {firstValueFrom, map} from "rxjs";
import {AuthenticationService} from "../services/authentication/authentication.service";

@Component({
  selector: 'app-view-reservation-details-dialog',
  templateUrl: './view-reservation-details-dialog.component.html',
  styleUrls: ['./view-reservation-details-dialog.component.scss']
})
export class ViewReservationDetailsDialogComponent implements OnInit{

  private readonly reservationId: string;
  public reservation: GetReservationByIdResponse | undefined;
  public canDelete = false;


  constructor(@Inject(MAT_DIALOG_DATA) public data: {reservationId: string},
              private readonly reservationService: ReservationService,
              private readonly dialogRef: MatDialogRef<ViewReservationDetailsDialogComponent>,
              private readonly authService: AuthenticationService) {
    this.reservationId = data.reservationId;
  }

  async ngOnInit(): Promise<void> {
    this.reservation = await firstValueFrom(this.reservationService.getReservationByIdEndpoint(this.reservationId));

    if (this.reservation) {
      this.canDelete = this.reservation.canDelete;
    }
  }

  async delete() {
    await firstValueFrom(this.reservationService.deleteReservationEndpoint(this.reservationId));
    this.dialogRef.close({
      reservationId: this.reservationId,
      wasDeleted: true
    });
  }

  getStatusText() : string {
    switch (this.reservation?.reservation.currentStatus) {
      case 'Pending':
        return 'Ausstehend';
      case 'Confirmed':
        return 'Bestätigt';
      case 'Denied':
        return 'Abgelehnt';
      default:
        return 'Unbekannt';
    }
  }
}
