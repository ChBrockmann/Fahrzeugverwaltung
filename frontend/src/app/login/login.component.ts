import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {DefaultService, TestService, UserService} from "../api";
import {firstValueFrom} from "rxjs";
import {AuthenticationService} from "../services/authentication/authentication.service";
import {Router} from "@angular/router";
import {KeycloakService} from "keycloak-angular";

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
              public readonly keycloakService: KeycloakService,
              private readonly testService: TestService,
              private readonly userService: UserService) {
    console.log(this.keycloakService.getUserRoles());
  }

  async keycloakLogin() {
    console.log("Keycloak login");
    await this.keycloakService.login();

  }

  async test2()
  {
    console.log(await this.keycloakService.isLoggedIn())
    console.log(await this.keycloakService.getToken());
    console.log(await this.keycloakService.loadUserProfile());
  }

  async test3()
  {
    this.testService.testEndpoint().subscribe(r => console.log(r));
  }

  async test() {
    let result = await firstValueFrom(this.loginService.postApiIdentityRefresh({
      refreshToken: "",
    }));
    console.log(result);
  }

  async login(): Promise<void> {
    this.status = Status.LOADING;
    try {
      let result = await firstValueFrom(this.loginService.postApiIdentityLogin(true, false, {
        email: this.loginFormGroup.value.email ?? "",
        password: this.loginFormGroup.value.password ?? ""
      }));

      this.status = Status.SUCCESS;

      let whoAmI = await firstValueFrom(this.userService.whoAmIEndpoint());
      this.authService.setUser(whoAmI);

      this.router.navigate([""]);
    } catch (e) {
      this.status = Status.ERROR;
    }
  }

}
