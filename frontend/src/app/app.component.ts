import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {ChatComponent} from "./components/main-layout/chat/chat.component";
import {SidebarComponent} from "./components/main-layout/sidebar/sidebar.component";
import {AccountComponent} from "./components/account/account.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ChatComponent, SidebarComponent, AccountComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Messaging Platform';
}
