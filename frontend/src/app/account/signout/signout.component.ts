import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-signout',
  standalone: true,
  imports: [],
  templateUrl: './signout.component.html',
  styleUrl: './signout.component.css'
})
export class SignOutComponent {
  constructor(private authService: AuthService, private router: Router) { }

  signOut() {
    this.authService.signOut().subscribe(success => {
      if (success) {
        this.router.navigate(['account/signin']);
      } else {
        console.error('Error occurred')
      }
    });
  }

}
