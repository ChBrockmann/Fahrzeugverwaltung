import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {VehicleModelDto, VehicleService} from "../api";
import * as console from "node:console";
import {firstValueFrom} from "rxjs";

@Component({
    selector: 'app-create-reservation-dialog',
    templateUrl: './create-reservation-dialog.component.html',
    styleUrls: ['./create-reservation-dialog.component.scss']
})
export class CreateReservationDialogComponent implements OnInit {
    createReservationFormGroup = new FormGroup({
        startDate: new FormControl('', [Validators.required]),
        endDate: new FormControl('', [Validators.required]),
        requestingUser: new FormControl('', [Validators.required])
    });
    private vehicles: VehicleModelDto[] | undefined;

    constructor(private readonly vehicleService: VehicleService) {

    }


    ngOnInit(): void {
    }

    // async loadDate(): Promise<void> {
    //     let vehicles = await firstValueFrom(this.vehicleService.getAllVehicleEndpoint());
    //     vehicles.vehicles
    // }


    click(): void {
        console.log(this.createReservationFormGroup.value);
    }
}
