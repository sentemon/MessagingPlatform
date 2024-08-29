import { Routes } from '@angular/router';
import { AccountComponent } from "./account/account.component";
import { SignInComponent } from "./account/signin/signin.component";
import { SignUpComponent } from "./account/signup/signup.component";
import { SignOutComponent } from "./account/signout/signout.component";
import {AuthGuard} from "./services/authguard/authguard.service";
import {SidebarComponent} from "./sidebar/sidebar.component";

export const routes: Routes = [
  { path: 'account',
    component: AccountComponent,
    children: [
      { path: 'signin', component: SignInComponent },
      { path: 'signup', component: SignUpComponent },
      { path: 'signout', component: SignOutComponent }
    ]},

  // if user unauthorized
  // { path: '', redirectTo: '/account/signin', pathMatch: 'full' },

  // if user authorized
  { path: '', component: SidebarComponent, canActivate: [AuthGuard] },
  { path: 'account/signout', component: SignOutComponent, canActivate: [AuthGuard] },

  // for invalid route
  { path: '**', redirectTo: '' },
];
