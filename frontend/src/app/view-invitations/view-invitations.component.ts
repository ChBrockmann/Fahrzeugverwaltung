import {Component, OnInit} from '@angular/core';
import {GetAllInvitationsResponse, InvitationModelDto, InvitationService} from "../api";
import {firstValueFrom} from "rxjs";
import * as moment from "moment/moment";

@Component({
  selector: 'app-view-invitations',
  templateUrl: './view-invitations.component.html',
  styleUrls: ['./view-invitations.component.scss']
})
export class ViewInvitationsComponent implements OnInit {

  public getAllInvitationResponse: GetAllInvitationsResponse | undefined;

  constructor(private readonly invitationService: InvitationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }

  async loadData(): Promise<void> {
    this.getAllInvitationResponse = await firstValueFrom(this.invitationService.getAllInvitationsEndpoint());
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

}
