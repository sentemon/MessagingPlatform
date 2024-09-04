import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignUpUser } from '../../models/signupuser';
import { SignInUser } from '../../models/signinuser';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {UserDto} from "../../models/userdto";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:8080/api/Account';

  constructor(private http: HttpClient) { }

  public signUp(user: SignUpUser): Observable<boolean> {
    return this.http.post<string>(`${this.apiUrl}/signup`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signIn(user: SignInUser): Observable<boolean> {
    return this.http.post<string>(`${this.apiUrl}/signin`, user, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public signOut(): Observable<boolean> {
    return this.http.post<string>(`${this.apiUrl}/signout`, null, { withCredentials: true })
      .pipe(
        map(() => true),
        catchError(() => of(false))
      );
  }

  public isAuthenticated(): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/isauthenticated`, { withCredentials: true })
      .pipe(
        catchError(() => of(false))
      );
  }

  public get(): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/get`, {withCredentials: true})
  }

  public getByUsername(username: string): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/getbyusername/${username}`, { withCredentials: true });
  }
}
