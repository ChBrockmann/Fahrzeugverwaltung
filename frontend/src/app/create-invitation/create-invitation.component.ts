import {Component, OnInit} from '@angular/core';
import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from 'moment';
import {InvitationService} from "../api";
import {MatDialogRef} from "@angular/material/dialog";
import {Router} from "@angular/router";

@Component({
  selector: 'app-create-invitation',
  templateUrl: './create-invitation.component.html',
  styleUrls: ['./create-invitation.component.scss']
})
export class CreateInvitationComponent implements OnInit {

  createInvitationFormGroup = new FormGroup({
    roles: new FormControl('none', {nonNullable: true}),
    count: new FormControl(1, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1), Validators.max(99)]
    }),
    expiresAt: new FormControl(moment().add(7, 'days'), {nonNullable: true, validators: [Validators.required]}),
    notes: new FormArray([])
  });

  constructor(private readonly invitationService: InvitationService,
              private readonly router: Router) {
  }

  ngOnInit(): void {
    this.updateNotes();
    this.createInvitationFormGroup.get('count')?.valueChanges.subscribe(() => {
      this.updateNotes();
    });
  }

  updateNotes(): void {
    const count = this.createInvitationFormGroup.get('count')?.value || 1;
    const notesArray = this.createInvitationFormGroup.get('notes') as FormArray;
    while (notesArray.length < count) {
      notesArray.push(new FormControl(''));
    }
    while (notesArray.length > count) {
      notesArray.removeAt(notesArray.length - 1);
    }
  }

  createInvitations(): void {
    console.log(this.createInvitationFormGroup.value);

    this.invitationService.createInvitationEndpoint({
      count: this.createInvitationFormGroup.value.count ?? 1,
      roles: this.mapRoles(),
      expiresAfterDay: this.createInvitationFormGroup.value.expiresAt?.add(1, 'day')?.toDate() ?? moment().add(7, 'days').toDate(),
      notes: this.createInvitationFormGroup.value.notes
    }).subscribe(data => {
      this.router.navigate(['/invitations']);
    });
  }

  mapRoles(): string[] {
    let selectedRole = this.createInvitationFormGroup.value.roles;

    if (selectedRole == null || selectedRole == 'none')
      return [];

    return [selectedRole];
  }

  get notesFormArray(): FormArray {
    return this.createInvitationFormGroup.get('notes') as FormArray;
  }
}
