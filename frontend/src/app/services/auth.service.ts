import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { SignUpUser } from "../models/signupuser";
import { SignInUser } from "../models/signinuser";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isLoggedIn: boolean = false;

  constructor(private http: HttpClient) { }

  public signUp(user: SignUpUser): Observable<string> {
    return this.http.post<string>("http://localhost:8080/api/Account/signup", user, {withCredentials: true });
  }

  public signIn(user: SignInUser): Observable<string> {
    return this.http.post<string>("http://localhost:8080/api/Account/signin", user, { withCredentials: true });
  }

  public signOut(): Observable<string> {
    return this.http.post<string>("http://localhost:8080/api/Account/signout", null, {withCredentials: true });
  }
}
