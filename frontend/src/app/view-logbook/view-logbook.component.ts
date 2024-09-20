import {Component, OnInit} from '@angular/core';
import {LogBookEntryDto, LogbookentryService, VehicleModelDto, VehicleService} from "../api";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-view-logbook',
  templateUrl: './view-logbook.component.html',
  styleUrls: ['./view-logbook.component.scss']
})
export class ViewLogbookComponent implements OnInit{

  public vehicles: VehicleModelDto[] | undefined;
  public selectedVehicleId: string | undefined;
  public logbookEntries: LogBookEntryDto[] | undefined;
  public displayedColumns: string[] = ['id', 'description', 'km', 'origin', 'destination'];


  constructor(private readonly vehicleService: VehicleService,
              private readonly logbookService: LogbookentryService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadVehicles();

    await this.loadLogbook(this.selectedVehicleId ?? "");
  }

  async loadVehicles(): Promise<void> {
    let result = await firstValueFrom(this.vehicleService.getAllVehicleEndpoint());
    this.vehicles = result.vehicles;

    this.selectedVehicleId = this.vehicles?.[0].id;
  }

  async loadLogbook(vehicleId: string): Promise<void> {
    let result = await firstValueFrom(this.logbookService.getAllLogBookEntriesForVehicleEndpoint(vehicleId));
    this.logbookEntries = result.logBookEntries;
  }
}
