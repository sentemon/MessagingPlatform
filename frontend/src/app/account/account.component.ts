import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import {SignOutComponent} from "./signout/signout.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [
    RouterOutlet,
    SignOutComponent,
  ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent { }
