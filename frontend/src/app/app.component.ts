import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { AuthService } from "./services/auth.service";
import { SignUpUser } from "./models/signupuser";
import { SignInUser } from "./models/signinuser";
import { FormsModule } from "@angular/forms";


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Messaging Platform';
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
