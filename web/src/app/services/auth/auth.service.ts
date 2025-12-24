import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignUpUser } from '../../models/requests/signupuser';
import { SignInUser } from '../../models/requests/signinuser';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {UserDto} from "../../models/responses/userdto";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public signUp(user: SignUpUser): Observable<boolean> {
    return this.http.post<string>(`${this.baseUrl}/account/signup`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signIn(user: SignInUser): Observable<boolean> {
    return this.http.post<string>(`${this.baseUrl}/account/signin`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signOut(): Observable<boolean> {
    return this.http.post<string>(`${this.baseUrl}/account/signout`, null, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/account/isauthenticated`, { withCredentials: true })
      .pipe(
        catchError(() => of(false))
      );
  }

  public get(): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.baseUrl}/account/me`, {withCredentials: true})
  }

  public getByUsername(username: string): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.baseUrl}/account/users/${username}`, { withCredentials: true });
  }
}
