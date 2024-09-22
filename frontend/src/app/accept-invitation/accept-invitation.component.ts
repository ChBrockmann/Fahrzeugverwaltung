import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, NonNullableFormBuilder, Validators} from "@angular/forms";
import {InvitationService, OrganizationDto, OrganizationService} from "../api";
import {firstValueFrom} from "rxjs";
import {HttpErrorResponse} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-accept-invitation',
  templateUrl: './accept-invitation.component.html',
  styleUrls: ['./accept-invitation.component.scss']
})
export class AcceptInvitationComponent implements OnInit{
  public acceptInvitationFormGroup: FormGroup;
  public isLoading = false;
  public isSuccessful = false;
  public errorText = '';
  public organizations: OrganizationDto[] = [];


  constructor(private readonly invitationService: InvitationService,
              private readonly nonNullableFormBuilder: NonNullableFormBuilder,
              private readonly route: ActivatedRoute,
              private readonly organizationService: OrganizationService) {
    this.acceptInvitationFormGroup = this.nonNullableFormBuilder.group({
      token: new FormControl(this.route.snapshot.queryParamMap.get('token') ?? '', [Validators.required]),
      firstname: new FormControl('', [Validators.required]),
      lastname: new FormControl('', [Validators.required]),
      organization: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
    });


  }

  async ngOnInit(): Promise<void> {
        await this.load();
  }

  async load() {
    this.organizations = (await firstValueFrom(this.organizationService.getAllOrganizationsEndpoint())).organizations ?? [];
  }


  async click() {
    console.log(this.acceptInvitationFormGroup.value);
    this.isLoading = true;
    let values = this.acceptInvitationFormGroup.value;
    let result = this.invitationService.acceptInvitationEndpoint({
      token: values.token,
      email: values.email,
      firstname: values.firstname,
      lastname: values.lastname,
      organization: values.organization,
      phoneNumber: values.phoneNumber
    })
      .subscribe({
        next: async data => {
          this.isSuccessful = true;
        },
        error: error => {
          this.handleError(error);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }

  handleError(error: HttpErrorResponse) {
    if (error.status == 500) {
      this.errorText = "Es ist ein unbekannter Fehler aufgetreten. Bitte versuchen Sie es später erneut.";
    } else {
      let errors = error.error.errors;
      if (errors?.passwordTooShort) {
        this.errorText = this.getGermanErrorText(errors.passwordTooShort[0]);
      }
      if(errors?.passwordRequiresLower) {
        this.errorText = this.getGermanErrorText(errors.passwordRequiresLower[0]);
      }
      if(errors?.passwordRequiresUpper) {
        this.errorText = this.getGermanErrorText(errors.passwordRequiresUpper[0]);
      }
      if(errors?.passwordRequiresDigit) {
        this.errorText = this.getGermanErrorText(errors.passwordRequiresDigit[0]);
      }
      if(errors?.token) {
        this.errorText = this.getGermanErrorText(errors.token[0]);
      }
      if(errors?.duplicateUserName) {
        this.errorText = this.getGermanErrorText(errors.duplicateUserName[0]);
      }
    }
  }

  getGermanErrorText(error: string) : string {
    //TODO: Implement
    switch (error) {
      default:
        return error;
    }
  }
}
