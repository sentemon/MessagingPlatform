import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-signout',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './signout.component.html',
  styleUrl: './signout.component.css'
})
export class SignOutComponent {
  constructor(private authService: AuthService, private router: Router) { }

  signOut(): void {
    this.authService.signOut().subscribe(success => {
      if (success) {
        this.router.navigate(['account/signin']);
      } else {
        console.error('Error occurred')
      }
    });
  }

}
