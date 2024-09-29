import {Component, OnInit} from '@angular/core';
import {OrganizationDto, OrganizationService} from "../../../api";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit {

  public organizations: OrganizationDto[] = [];
  public displayedColumns: string[] = ['name', 'user-count', 'basic-edit'];
  public isLoading = true;

  constructor(private readonly organizationService: OrganizationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  async loadData(): Promise<void> {
    this.isLoading = true;
    this.organizations = (await firstValueFrom(this.organizationService.getAllOrganizationsEndpoint())).organizations;
    this.isLoading = false;
  }
}
