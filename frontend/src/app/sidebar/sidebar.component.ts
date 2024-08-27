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
      {firstName: "Jane", lastName: "January", avatar: "avatar.svg"},
      {firstName: "Female", lastName: "Female", avatar: "avatar.svg"},
      {firstName: "Male", lastName: "Male", avatar: "avatar.svg"},
      {firstName: "John", lastName: "John", avatar: "avatar.svg"},
      {firstName: "Peter", lastName: "Parker", avatar: "avatar.svg"},
      {firstName: "Roman", lastName: "Roman", avatar: "avatar.svg"},
      {firstName: "Sain", lastName: "Sain", avatar: "avatar.svg"},
      {firstName: "Gol", lastName: "Med", avatar: "avatar.svg"},
      {firstName: "Ericaaaaaaaaaaaaaaaaaaaaaaaaaa", lastName: "Eric", avatar: "avatar.svg"},
      {firstName: "French", lastName: "French", avatar: "avatar.svg"},
      {firstName: "Chico", lastName: "Lachowski", avatar: "avatar.svg"},
      {firstName: "Marek", lastName: "Male", avatar: "avatar.svg"},
      {firstName: "John", lastName: "John", avatar: "avatar.svg"},
      {firstName: "Alley", lastName: "Downey", avatar: "avatar.svg"},
  ]
}
