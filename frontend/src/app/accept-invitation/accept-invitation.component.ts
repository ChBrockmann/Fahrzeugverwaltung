import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, NonNullableFormBuilder, Validators} from "@angular/forms";
import {InvitationService} from "../api";
import {firstValueFrom} from "rxjs";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {HttpErrorResponse} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-accept-invitation',
  templateUrl: './accept-invitation.component.html',
  styleUrls: ['./accept-invitation.component.scss']
})
export class AcceptInvitationComponent {
  public acceptInvitationFormGroup: FormGroup;
  public isLoading = false;
  public errorText = '';


  constructor(private readonly invitationService: InvitationService,
              private readonly nonNullableFormBuilder: NonNullableFormBuilder,
              private readonly route: ActivatedRoute) {
    this.acceptInvitationFormGroup = this.nonNullableFormBuilder.group({
      token: new FormControl(this.route.snapshot.queryParamMap.get('token') ?? '', [Validators.required]),
      firstname: new FormControl('', [Validators.required]),
      lastname: new FormControl('', [Validators.required]),
      organization: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });
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
      password: values.password
    })
      .subscribe({
        next: data => {
          //Happy path
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

  handleError(error: Error) {
    if (error instanceof HttpErrorResponse) {
      this.errorText = JSON.stringify(error.error.errors);
    }
    else {
      this.errorText = error.message;
    }
  }
}
