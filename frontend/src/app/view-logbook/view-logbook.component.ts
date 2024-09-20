import {Component, OnInit} from '@angular/core';
import {LogBookEntryDto, LogbookentryService, VehicleModelDto, VehicleService} from "../api";
import {firstValueFrom} from "rxjs";
import {CreateManualLogbookEntryComponent} from "../create-manual-logbook-entry/create-manual-logbook-entry.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-view-logbook',
  templateUrl: './view-logbook.component.html',
  styleUrls: ['./view-logbook.component.scss']
})
export class ViewLogbookComponent implements OnInit{

  public vehicles: VehicleModelDto[] | undefined;
  public selectedVehicle: VehicleModelDto | undefined;
  public logbookEntries: LogBookEntryDto[] | undefined;
  public displayedColumns: string[] = ['currentNumber', 'description', 'endMileageInKm'];


  constructor(private readonly vehicleService: VehicleService,
              private readonly logbookService: LogbookentryService,
              private readonly dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadVehicles();

    await this.loadLogbook(this.selectedVehicle);
  }

  async loadVehicles(): Promise<void> {
    let result = await firstValueFrom(this.vehicleService.getAllVehicleEndpoint());
    this.vehicles = result.vehicles;

    this.selectedVehicle = this.vehicles?.[0];
  }

  async loadLogbook(vehicle: VehicleModelDto | undefined): Promise<void> {
    if(vehicle === undefined) {
      return;
    }

    let result = await firstValueFrom(this.logbookService.getAllLogBookEntriesForVehicleEndpoint(vehicle.id ?? ""));
    this.logbookEntries = result.logBookEntries;
  }

  createNewEntry() {
    this.dialog.open(CreateManualLogbookEntryComponent, {data: {vehicle: this.selectedVehicle}})
      .afterClosed()
      .subscribe((data) => {
      console.log(data);
    });
  }

}
