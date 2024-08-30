import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import {SignOutComponent} from "./signout/signout.component";
import {SidebarComponent} from "../sidebar/sidebar.component";
import {ChatComponent} from "../chat/chat.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [
    RouterOutlet,
    SignOutComponent,
    SidebarComponent,
    ChatComponent,
  ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent { }
