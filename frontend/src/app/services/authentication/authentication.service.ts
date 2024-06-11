import {Injectable} from '@angular/core';
import {UserDto, WhoAmIResponse} from "../../api";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly userKey = "user";

  constructor() {
  }


  public clear(): void {
    localStorage.clear();
  }

  public setUser(user: WhoAmIResponse) {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  public getUser(): WhoAmIResponse | null {
    const user = localStorage.getItem(this.userKey);
    if (user) {
      return JSON.parse(user);
    }
    return null;
  }

  public hasUser() : boolean {
    return this.getUser() !== null;
  }
}
