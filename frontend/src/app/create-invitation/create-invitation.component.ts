import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from 'moment';
import {InvitationService} from "../api";

@Component({
  selector: 'app-create-invitation',
  templateUrl: './create-invitation.component.html',
  styleUrls: ['./create-invitation.component.scss']
})
export class CreateInvitationComponent {

  createInvitationFormGroup = new FormGroup({
    roles: new FormControl('none', {nonNullable: true}),
    count: new FormControl(1, { nonNullable: true, validators: [Validators.required, Validators.min(1), Validators.max(99)]}),
    expiresAt: new FormControl(moment().add(7, 'days'), {nonNullable: true, validators: [Validators.required]})
  });

  constructor(private readonly invitationService: InvitationService) {
  }

  createInvitations() : void {
    console.log(this.createInvitationFormGroup.value)

    this.invitationService.createInvitationEndpoint({
      count: this.createInvitationFormGroup.value.count ?? 1,
      roles: this.mapRoles(),
      expiresAfterDay: this.createInvitationFormGroup.value.expiresAt?.toDate() ?? moment().add(7, 'days').toDate()
    }, "response", false ).subscribe(data => {
      console.log("TEPKIJAoksdjokjds");
      console.log(data);
    });
  }

  mapRoles() : string[] {
    let selectedRole = this.createInvitationFormGroup.value.roles;

    if(selectedRole == null || selectedRole == 'none')
      return [];

    return [selectedRole];
  }
}
