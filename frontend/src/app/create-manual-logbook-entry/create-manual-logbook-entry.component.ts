import {Component, Inject} from '@angular/core';
import {LogbookentryService, VehicleModelDto} from "../api";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from "moment/moment";
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-create-manual-logbook-entry',
  templateUrl: './create-manual-logbook-entry.component.html',
  styleUrls: ['./create-manual-logbook-entry.component.scss']
})
export class CreateManualLogbookEntryComponent {

  public vehicle: VehicleModelDto;
  createManualLogbookEntryFormGroup = new FormGroup({
    description: new FormControl<string>(""),
    totalMileageInKm: new FormControl<number>(0, {nonNullable: true, validators: [Validators.required, Validators.min(0)]}),
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: { vehicle: VehicleModelDto },
              private readonly dialogRef: MatDialogRef<CreateManualLogbookEntryComponent>,
              private readonly logbookEntryService: LogbookentryService) {
    this.vehicle = data.vehicle;
  }

  saveAndClose()
  {
    this.logbookEntryService.createManualLogBookEntry(this.vehicle.id ?? "", {
      description: this.createManualLogbookEntryFormGroup.value.description,
      totalMileageInKm: this.createManualLogbookEntryFormGroup.value.totalMileageInKm ?? 0
    }).subscribe((result) => {
      this.dialogRef.close(result);
    })
  }
}
