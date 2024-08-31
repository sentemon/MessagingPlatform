import { Component } from '@angular/core';
import {SidebarComponent} from "./sidebar/sidebar.component";
import {ChatComponent} from "./chat/chat.component";

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    SidebarComponent,
    ChatComponent
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {

}
