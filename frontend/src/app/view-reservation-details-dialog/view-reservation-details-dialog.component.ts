import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {ReservationModelDto, ReservationService} from "../api";
import {firstValueFrom, map} from "rxjs";

@Component({
  selector: 'app-view-reservation-details-dialog',
  templateUrl: './view-reservation-details-dialog.component.html',
  styleUrls: ['./view-reservation-details-dialog.component.scss']
})
export class ViewReservationDetailsDialogComponent implements OnInit{

  private readonly reservationId: string;
  public reservation: ReservationModelDto | undefined;


  constructor(@Inject(MAT_DIALOG_DATA) public data: {reservationId: string},
              private readonly reservationService: ReservationService) {
    this.reservationId = data.reservationId;
  }

  async ngOnInit(): Promise<void> {
    this.reservation = await firstValueFrom(this.reservationService.getReservationByIdEndpoint(this.reservationId).pipe(map(r => r.reservation)));
  }
}
