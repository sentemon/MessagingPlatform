import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../../services/auth/auth.service";

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {
  constructor(private authService: AuthService, private router: Router) { }

  onSignOut() {
    this.authService.signOut().subscribe(success => {
      if (success) {
        this.router.navigate(['account/signin']).then(r => console.log('Navigated to signin', r));
      } else {
        console.error('Error occurred');
        }
    });
  }
}

