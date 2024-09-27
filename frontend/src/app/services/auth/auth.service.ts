import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignUpUser } from '../../models/signupuser';
import { SignInUser } from '../../models/signinuser';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {UserDto} from "../../models/userdto";
import { environment } from "../../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  public signUp(user: SignUpUser): Observable<boolean> {
    return this.http.post<string>(`${environment.apiUrl}/Account/signup`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signIn(user: SignInUser): Observable<boolean> {
    return this.http.post<string>(`${environment.apiUrl}/Account/signin`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signOut(): Observable<boolean> {
    return this.http.post<string>(`${environment.apiUrl}/Account/signout`, null, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/Account/isauthenticated`, { withCredentials: true })
      .pipe(
        catchError(() => of(false))
      );
  }

  public get(): Observable<UserDto> {
    return this.http.get<UserDto>(`${environment.apiUrl}/Account/get`, {withCredentials: true})
  }

  public getByUsername(username: string): Observable<UserDto> {
    return this.http.get<UserDto>(`${environment.apiUrl}/Account/getbyusername/${username}`, { withCredentials: true });
  }
}
