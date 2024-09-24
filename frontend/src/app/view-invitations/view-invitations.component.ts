import {Component, OnInit} from '@angular/core';
import {GetAllInvitationsResponse, InvitationModelDto, InvitationService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment/moment";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {CreateInvitationComponent} from "../create-invitation/create-invitation.component";
import {KeycloakService} from "keycloak-angular";

@Component({
  selector: 'app-view-invitations',
  templateUrl: './view-invitations.component.html',
  styleUrls: ['./view-invitations.component.scss']
})
export class ViewInvitationsComponent implements OnInit {

  public getAllInvitationResponse: GetAllInvitationsResponse | undefined;
  public OnlyShowOwnInvitations = true;
  public OnlyShowTypeOfInvitations: "All" | "NonAccepted" | "Accepted" = "All";

  private userId: string | undefined;

  constructor(private readonly invitationService: InvitationService,
              private readonly keycloak: KeycloakService,
              public dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  async loadData(): Promise<void> {
    this.getAllInvitationResponse = undefined;
    this.userId = (await this.keycloak.loadUserProfile()).id;
    this.getAllInvitationResponse = await firstValueFrom(this.invitationService.getAllInvitationsEndpoint());
  }

  filter(): InvitationModelDto[] {
    let invitations = this.getAllInvitationResponse?.invitations ?? [];

    return invitations.filter(dto => {
      if(this.OnlyShowOwnInvitations && dto.createdBy !== undefined && dto.createdBy?.authId?.toLowerCase() == this.userId?.toLowerCase()) {
        return false;
      }
      if(this.OnlyShowTypeOfInvitations === "Accepted" && dto.acceptedBy == null) {
        return false;
      }
      if(this.OnlyShowTypeOfInvitations === "NonAccepted" && dto.acceptedBy != null) {
        return false;
      }
      return true;
    }) ?? [];
  }

  createInvitation() {
    this.dialog.open<CreateInvitationComponent>(CreateInvitationComponent)
      .afterClosed().subscribe(async () => {
        await this.loadData();
    });
  }


  getStatusText(invitation: InvitationModelDto) : "Angenommen" | "Offen" | "Abgelaufen" {
    if(invitation.acceptedBy != null) {
      return "Angenommen";
    }
    if(invitation.expiresAt == null || moment(invitation.expiresAt) < moment().utc()) {
      return "Abgelaufen";
    }
    return "Offen";
  }

  getTypeText(invitation: InvitationModelDto) : string {
    if(invitation.roles == undefined || invitation.roles.length === 0) {
      return "Standard";
    }
    return invitation.roles.join(", ").trimEnd();
  }

  loadingPdfs: string[] = [];

  isDisabled(invitationId: string | undefined) : boolean {
    if(invitationId == null) {
      return true;
    }
    return this.loadingPdfs.includes(invitationId);
  }

  downloadInvitationPdf(id: string | undefined) : void {
    if(id == null) {
      return;
    }
    this.loadingPdfs.push(id);
    this.invitationService.getInvitationPdfEndpoint(id, window.location.origin).subscribe(data => {
      console.log(data)
      // @ts-ignore
      const file = new Blob([data], {type: 'application/pdf'});
      const fileURL = URL.createObjectURL(file);
      window.open(fileURL, '_blank')?.focus();
      this.loadingPdfs = this.loadingPdfs.filter(value => value !== id);
    });
  }


}
