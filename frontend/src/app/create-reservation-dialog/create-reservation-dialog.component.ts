import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {VehicleModelDto, VehicleService} from "../api";

@Component({
  selector: 'app-create-reservation-dialog',
  templateUrl: './create-reservation-dialog.component.html',
  styleUrls: ['./create-reservation-dialog.component.scss']
})
export class CreateReservationDialogComponent {

  createReservationFormGroup = new FormGroup({
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl('', [Validators.required]),
    requestingUser: new FormControl('', [Validators.required])
  });
  private vehicles: VehicleModelDto[] | undefined;

  constructor(private readonly vehicleService: VehicleService) {

  }

  click(): void {
    console.log(this.createReservationFormGroup.value);
  }

}
