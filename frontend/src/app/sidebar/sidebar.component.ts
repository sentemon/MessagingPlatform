import { Component } from '@angular/core';
import {User} from "../models/user";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  users: User[] = [
      {firstName: "Ivan", lastName: "Sentemon", avatar: "avatar.svg"},
      {firstName: "Nom", lastName: "Nometnes", avatar: "avatar.svg"},

  ]
}
