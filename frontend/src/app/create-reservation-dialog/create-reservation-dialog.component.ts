import {Component, Inject, OnInit, Optional} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, Validators} from "@angular/forms";
import {ReservationService, VehicleModelDto, VehicleService} from "../api";
import {firstValueFrom, Observable} from "rxjs";
import * as moment from "moment";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CheckReservationStatus} from "../validator/check-reservation-status/check-reservation-status";
import {AuthenticationService} from "../services/authentication/authentication.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-create-reservation-dialog',
  templateUrl: './create-reservation-dialog.component.html',
  styleUrls: ['./create-reservation-dialog.component.scss']
})
export class CreateReservationDialogComponent implements OnInit {

  createReservationFormGroup = new FormGroup({
    startDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
    endDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
    requestedVehicleId: new FormControl('', [Validators.required]),
  });
  public vehicles: VehicleModelDto[] | undefined;
  public errorText = '';

  constructor(private readonly vehicleService: VehicleService,
              private readonly reservationService: ReservationService,
              private readonly dialogRef: MatDialogRef<CreateReservationDialogComponent>,
              @Optional() @Inject(MAT_DIALOG_DATA) public data: {
                startDate: moment.Moment,
                endDate: moment.Moment
              } | undefined) {

    if (data !== null && data !== undefined) {
      this.createReservationFormGroup.patchValue({
        startDate: data.startDate,
        endDate: data.endDate
      })
    }

    this.createReservationFormGroup.addAsyncValidators(CheckReservationStatus.checkVehicleAvailability(this.reservationService));
  }

  async ngOnInit(): Promise<void> {
    this.vehicles = (await firstValueFrom(this.vehicleService.getAllVehicleEndpoint())).vehicles;

    if (this.vehicles) {
      this.createReservationFormGroup.patchValue({requestedVehicleId: this.vehicles[0].id});
    }
  }

  createReservation(): void {
    let formValues = this.createReservationFormGroup.value;
    this.reservationService.createReservationEndpoint({
      startDateInclusive: formValues.startDate?.format("YYYY-MM-DD") ?? "",
      endDateInclusive: formValues.endDate?.format("YYYY-MM-DD") ?? "",
      vehicle: formValues.requestedVehicleId ?? "",
    }).subscribe({
      next: (response) => {
        this.dialogRef.close(response);
      },
      error: (error: HttpErrorResponse) => {
        this.handleError(error);
      }
    });
  }

  handleError(error: HttpErrorResponse) {
    if (error.status == 500) {
      this.errorText = "Es ist ein unbekannter Fehler aufgetreten. Bitte versuchen Sie es später erneut.";
    } else {
      let errors = error.error.errors;
      if (errors?.startDateInclusive) {
        this.errorText = this.getGermanErrorText(errors.startDateInclusive[0]);
      }
      if(errors?.vehicle) {
        this.errorText = this.getGermanErrorText(errors.vehicle[0]);
      }
    }
  }

  getGermanErrorText(error: string) : string {
    switch (error) {
      default:
        return error;
    }
  }
}
