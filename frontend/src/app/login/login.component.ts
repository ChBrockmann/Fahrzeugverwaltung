import { Component } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {DefaultService} from "../api";
import {firstValueFrom} from "rxjs";
import {AuthenticationService} from "../services/authentication/authentication.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginFormGroup = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.required])
  })

  constructor(private readonly loginService: DefaultService,
              private readonly authService: AuthenticationService) {
  }

  async login(): Promise<void> {
    console.log(this.loginFormGroup.value);
    let result = await firstValueFrom(this.loginService.postApiIdentityLogin(false, false, {
      email: this.loginFormGroup.value.email ?? "",
      password: this.loginFormGroup.value.password ?? ""
    }));
    console.log(result);
    this.authService.setToken(result.accessToken ?? "");
  }

}
