import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {DefaultService, UserService} from "../api";
import {firstValueFrom} from "rxjs";
import {AuthenticationService} from "../services/authentication/authentication.service";
import {Router} from "@angular/router";

enum Status {
  SUCCESS = "success",
  ERROR = "error",
  LOADING = "loading",
  IDLE = "idle"
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  public status: Status = Status.IDLE;

  loginFormGroup = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.required])
  })

  constructor(private readonly loginService: DefaultService,
              private readonly authService: AuthenticationService,
              private readonly router: Router,
              private readonly userService: UserService) {
  }

  async login(): Promise<void> {
    this.status = Status.LOADING;
    try {
      let result = await firstValueFrom(this.loginService.postApiIdentityLogin(false, false, {
        email: this.loginFormGroup.value.email ?? "",
        password: this.loginFormGroup.value.password ?? ""
      }));

      if (result.accessToken) {
        this.status = Status.SUCCESS;
        this.authService.setToken(result.accessToken ?? "");

        this.router.navigate([""]);
      } else {
        this.status = Status.ERROR;
      }
    }
    catch(e) {
      this.status = Status.ERROR;
    }
  }

}
