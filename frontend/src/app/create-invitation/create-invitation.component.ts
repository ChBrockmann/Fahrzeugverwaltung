import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from 'moment';
import {InvitationService} from "../api";
import {MatDialogRef} from "@angular/material/dialog";

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

  constructor(private readonly invitationService: InvitationService,
              private readonly dialogRef: MatDialogRef<CreateInvitationComponent>) {
  }

  createInvitations() : void {
    console.log(this.createInvitationFormGroup.value)

    this.invitationService.createInvitationEndpoint({
      count: this.createInvitationFormGroup.value.count ?? 1,
      roles: this.mapRoles(),
      expiresAfterDay: this.createInvitationFormGroup.value.expiresAt?.add(1, 'day')?.toDate() ?? moment().add(7, 'days').toDate()
    }).subscribe(data => {
      this.dialogRef.close();
    });
  }

  mapRoles() : string[] {
    let selectedRole = this.createInvitationFormGroup.value.roles;

    if(selectedRole == null || selectedRole == 'none')
      return [];

    return [selectedRole];
  }
}
