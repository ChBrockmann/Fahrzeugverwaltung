import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, NonNullableFormBuilder, Validators} from "@angular/forms";
import {DefaultService, InvitationService, UserService} from "../api";
import {firstValueFrom} from "rxjs";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {HttpErrorResponse} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../services/authentication/authentication.service";

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
              private readonly route: ActivatedRoute,
              private readonly router: Router,
              private readonly loginService: DefaultService,
              private readonly authService: AuthenticationService,
              private readonly userService: UserService) {
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
      password: values.password,
      phoneNumber: ''
    })
      .subscribe({
        next: async data => {
          await this.login(values.email, values.password);
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

  async login(email: string, password: string): Promise<void> {
    try {
      let result = await firstValueFrom(this.loginService.postApiIdentityLogin(true, false, {
        email: email ?? "",
        password: password ?? ""
      }));

      let whoAmI = await firstValueFrom(this.userService.whoAmIEndpoint());
      this.authService.setUser(whoAmI);

      this.router.navigate([""]);
    } catch (e) {
      this.errorText = "Anmeldung fehlgeschlagen.";
    }
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
