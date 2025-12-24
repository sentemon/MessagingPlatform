import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserDto } from "../../../models/responses/userdto";
import { AuthService } from "../../../services/auth/auth.service";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { FormsModule } from "@angular/forms";
import { AsyncPipe, NgIf } from "@angular/common";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    AsyncPipe
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  public user$!: Observable<UserDto>;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const username = params.get('username');
      if (username) {
        this.user$ = this.authService.getByUsername(username).pipe(
          map(user => user)
        );
      }
    });
  }
}
