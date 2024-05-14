import {Injectable} from '@angular/core';
import {UserDto} from "../../api";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly tokenKey = "token";
  private readonly userKey = "user";

  constructor() {
  }

  public setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public hasToken(): boolean {
    return this.getToken() !== null;
  }

  public claer(): void {
    localStorage.clear();
  }

  public setUser(user: UserDto): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  public getUser(): UserDto | null {
    let user = localStorage.getItem(this.userKey);
    if (user) {
      return JSON.parse(user);
    }
    return null;
  }

  public getUserId() : string | null {
    let user = this.getUser();
    if (user) {
      return user.id ?? "";
    }
    return null;
  }
}
