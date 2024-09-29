import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationManagementComponent } from './organization-management/organization-management.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatTableModule} from "@angular/material/table";
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";


@NgModule({
  declarations: [
    OrganizationManagementComponent,
    OrganizationListComponent
  ],
  imports: [
    CommonModule,
    OrganizationRoutingModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class OrganizationModule { }
