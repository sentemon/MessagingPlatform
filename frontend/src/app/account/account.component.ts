import { Component } from '@angular/core';
import { SignUpUser } from "../models/signupuser";
import { AuthService } from "../services/auth.service";
import { SignInUser } from "../models/signinuser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgIf } from "@angular/common";
import {RouterLink, RouterOutlet} from "@angular/router";
import {SignInComponent} from "./signin/signin.component";
import {SignUpComponent} from "./signup/signup.component";
import {SignOutComponent} from "./signout/signout.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    RouterOutlet,
    SignInComponent,
    SignUpComponent,
    SignOutComponent,
    RouterLink
  ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {
  signUpUser = new SignUpUser();

  constructor(private authService: AuthService) { }

  signUp(user: SignUpUser) {
    this.authService.signUp(user).subscribe((str: string) => {
      console.log(str);
    });
  }

  signIn(user: SignInUser) {
    this.authService.signIn(user).subscribe((str: string) => {
      console.log(str);
    });
  }

  signOut() {
    this.authService.signOut().subscribe((str: string) => {
      console.log(str);
    });
  }
}
