import { Routes } from '@angular/router';
import { AccountComponent } from "./account/account.component";
import { SignInComponent } from "./account/signin/signin.component";
import { SignUpComponent } from "./account/signup/signup.component";
import { SignOutComponent } from "./account/signout/signout.component";
import {AuthGuard} from "./services/authguard/authguard.service";
import {SidebarComponent} from "./main-layout/sidebar/sidebar.component";
import {ChatComponent} from "./main-layout/chat/chat.component";
import {NonAuthGuard} from "./services/nonauthguard/nonauthguard.service";
import {MainLayoutComponent} from "./main-layout/main-layout.component";


export const routes: Routes = [
  { path: 'account',
    component: AccountComponent,
    children: [
      { path: 'signin', component: SignInComponent, canActivate: [NonAuthGuard] },
      { path: 'signup', component: SignUpComponent, canActivate: [NonAuthGuard] },
      { path: 'signout', component: SignOutComponent, canActivate: [AuthGuard] }, // fix
    ]},


  // if user unauthorized
  // { path: '', redirectTo: '/account/signin', pathMatch: 'full' },

  // if user authorized
  { path: '', component: MainLayoutComponent, canActivate: [AuthGuard] },


  // for invalid route
  { path: '**', redirectTo: '' },
];
