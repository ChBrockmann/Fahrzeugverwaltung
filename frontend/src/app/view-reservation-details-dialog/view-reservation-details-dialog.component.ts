import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {ReservationService} from "../api";

@Component({
  selector: 'app-view-reservation-details-dialog',
  templateUrl: './view-reservation-details-dialog.component.html',
  styleUrls: ['./view-reservation-details-dialog.component.scss']
})
export class ViewReservationDetailsDialogComponent implements OnInit{

  constructor(@Inject(MAT_DIALOG_DATA) public data: {reservationId: string},
              private readonly reservationService: ReservationService) {
  }

  ngOnInit(): void {

    }
}
