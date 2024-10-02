import { Component } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import {SidebarComponent} from "../main-layout/sidebar/sidebar.component";
import {ChatComponent} from "../main-layout/chat/chat.component";

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [
    RouterOutlet,
    SidebarComponent,
    ChatComponent,
  ],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent { }
