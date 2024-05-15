import {Injectable} from '@angular/core';
import {UserDto} from "../../api";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private readonly tokenKey = "token";

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
}
