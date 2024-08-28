import { Component } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { SignInUser } from "../../models/signinuser";
import {NgIf} from "@angular/common";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [
    FormsModule,
    NgIf
  ],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css'
})
export class SignInComponent {
  signInUser = new SignInUser();

  constructor(private authService: AuthService, private router: Router) { }

  signIn(user: SignInUser) {
    this.authService.signIn(user).subscribe(success => {
      if (success) {
        this.router.navigate(['/']);
      } else {
        console.error("Error occurred");
      }
    });
  }
}
