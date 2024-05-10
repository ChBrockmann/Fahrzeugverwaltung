import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ReservationService, VehicleModelDto, VehicleService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment";

@Component({
  selector: 'app-create-reservation-dialog',
  templateUrl: './create-reservation-dialog.component.html',
  styleUrls: ['./create-reservation-dialog.component.scss']
})
export class CreateReservationDialogComponent implements OnInit {

  createReservationFormGroup = new FormGroup({
    startDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
    endDate: new FormControl<moment.Moment>(moment.utc(), [Validators.required]),
    requestingUser: new FormControl('4098D24F-0A17-4BAA-B895-B5C6E3926DE7', [Validators.required]),
    requestedVehicleId: new FormControl('', [Validators.required]),
  });
  public vehicles: VehicleModelDto[] | undefined;

  constructor(private readonly vehicleService: VehicleService,
              private readonly reservationService: ReservationService) {

  }

  async ngOnInit(): Promise<void> {
    this.vehicles = (await firstValueFrom(this.vehicleService.getAllVehicleEndpoint())).vehicles;

    if (this.vehicles) {
      this.createReservationFormGroup.patchValue({requestedVehicleId: this.vehicles[0].id});
    }

    console.log(this.vehicles);
  }

  click(): void {
    console.log(this.createReservationFormGroup.value);
  }

  createReservation() : void {
    let formValues = this.createReservationFormGroup.value;
    this.reservationService.createReservationEndpoint({
      reservedBy: formValues.requestingUser ?? "",
      startDateInclusive: formValues.startDate?.format("YYYY-MM-DD") ?? "",
      endDateInclusive: formValues.endDate?.format("YYYY-MM-DD") ?? "",
      vehicle: formValues.requestedVehicleId ?? "",
    }).subscribe(() => {});
  }

}
