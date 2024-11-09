import {Component, Inject, OnInit, Optional} from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, Validators} from "@angular/forms";
import {ReservationService, VehicleModelDto, VehicleService} from "../api";
import {firstValueFrom, Observable} from "rxjs";
import * as moment from "moment";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CheckReservationStatus} from "../validator/check-reservation-status/check-reservation-status";
import {HttpErrorResponse} from "@angular/common/http";
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-create-reservation-dialog',
  templateUrl: './create-reservation-dialog.component.html',
  styleUrls: ['./create-reservation-dialog.component.scss']
})
export class CreateReservationDialogComponent implements OnInit {

  createReservationFormGroup = new FormGroup({
    asyncForm: new FormGroup({
      startDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
      endDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
      requestedVehicleId: new FormControl('', [Validators.required])
    }),
    originAdress: new FormControl('', [Validators.required, Validators.maxLength(512)]),
    destinationAdress: new FormControl('', [Validators.required, Validators.maxLength(512)]),
    reason: new FormControl(null!, {nonNullable: true, validators: [Validators.required]})
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
    this.createReservationFormGroup.patchValue({
      originAdress: environment.defaults.originAdress
    });

    if (data !== null && data !== undefined) {
      this.createReservationFormGroup.patchValue({
        asyncForm: {
          startDate: data.startDate,
          endDate: data.endDate
        }
      })
    }

    this.createReservationFormGroup.get('asyncForm')?.addAsyncValidators(CheckReservationStatus.checkVehicleAvailability(this.reservationService));
  }

  async ngOnInit(): Promise<void> {
    this.vehicles = (await firstValueFrom(this.vehicleService.getAllVehicleEndpoint())).vehicles;

    if (this.vehicles) {
      this.createReservationFormGroup.patchValue({asyncForm: {requestedVehicleId: this.vehicles[0].id}});
    }
  }

  createReservation(): void {
    let formValues = this.createReservationFormGroup.value;
    this.reservationService.createReservationEndpoint({
      startDateInclusive: formValues.asyncForm?.startDate?.format("YYYY-MM-DD") ?? "",
      endDateInclusive: formValues.asyncForm?.endDate?.format("YYYY-MM-DD") ?? "",
      vehicle: formValues.asyncForm?.requestedVehicleId ?? "",
      reason: formValues.reason ?? "",
      originAdress: formValues.originAdress ?? "",
      destinationAdress: formValues.destinationAdress ?? ""
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
      if (errors?.vehicle) {
        this.errorText = this.getGermanErrorText(errors.vehicle[0]);
      }
    }
  }

  getGermanErrorText(error: string): string {
    //TODO: Implement localization
    switch (error) {
      default:
        return error;
    }
  }

  getErrorText(): string {
    let validationErrors: ValidationErrors | null = this.createReservationFormGroup.controls.reason?.errors;

    if (validationErrors == null) {
      return "";
    } else {
      if (validationErrors['required']) {
        return "Grund für die Reservierung ist erforderlich";
      } else {
        return validationErrors.toString();
      }
    }
  }

  localizeAsyncValidationErrors(): string[] {
    let validationErrors: string[] = this.createReservationFormGroup.controls.asyncForm.errors?.['validationErrors'];

    let localizedValidationErrors: string[] = [];
    for (let validationError of validationErrors) {
      localizedValidationErrors.push(this.localizeErrorMessage(validationError));
    }

    return localizedValidationErrors;
  }

  private localizeErrorMessage(error: string): string {
    switch (error) {
      default:
        return "Es ist ein unbekannter Fehler aufgetreten:\n" + error;
      case "Startdate has to be before Enddate":
        return "Startdatum muss vor Enddatum liegen";
      case "Startdate has to be after today":
        return "Startdatum muss in der Zukunft liegen";
      case "Reservation exceeds maximum reservation days":
        return "Reservierungsdauer überschreitet maximale Reservierungsdauer";
      case "Reservation is below minimum reservation days":
        return "Reservierungsdauer liegt unterhalb der minimalen Reservierungsdauer";
      case "Reservation is below minimum reservation time in advance":
        return "Die Reservierung ist zu kurzfristig";
      case "Reservation is above maximum reservation time in advance":
        return "Die Reservierung liegt zu weit in der Zukunft";
      case "Vehicle is already reserved during (part of) this timespan":
        return "Fahrzeug ist bereits (teilweise) während dieses Zeitraums reserviert";
    }
  }
}
