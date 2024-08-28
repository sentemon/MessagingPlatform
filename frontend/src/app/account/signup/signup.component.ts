import { Component } from '@angular/core';
import {SignUpUser} from "../../models/signupuser";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    NgIf,
    FormsModule
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignUpComponent {
  signUpUser = new SignUpUser();

  constructor(private authService: AuthService, private router: Router) { }

  signUp(user: SignUpUser) {
    this.authService.signUp(user).subscribe((str: string) => {
      console.log(str);
      this.router.navigate(['/']);
    })
  }

  protected readonly SignUpUser = SignUpUser;
}
