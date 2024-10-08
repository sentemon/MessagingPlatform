import { Routes } from '@angular/router';
import { AccountComponent } from "./components/account/account.component";
import { SignInComponent } from "./components/account/signin/signin.component";
import { SignUpComponent } from "./components/account/signup/signup.component";
import { AuthGuard } from "./services/authguard/authguard.service";
import { NonAuthGuard } from "./services/nonauthguard/nonauthguard.service";
import { MainLayoutComponent } from "./components/main-layout/main-layout.component";
import { ProfileComponent } from "./components/account/profile/profile.component";
import {SettingsComponent} from "./components/settings/settings.component";

export const routes: Routes = [
  {
    path: 'account',
    component: AccountComponent,
    children: [
      { path: 'signin', component: SignInComponent, canActivate: [NonAuthGuard] },
      { path: 'signup', component: SignUpComponent, canActivate: [NonAuthGuard] },
      { path: '', redirectTo: 'signin', pathMatch: 'full' } // Default route for 'account'
    ]
  },

  // Route for profile with username parameter
  {
    // ToDo: make profile component
    path: 'profile/:username',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'settings',
    component: SettingsComponent,
    canActivate: [AuthGuard]
  },

  // Default route if user is authorized
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard]
  },

  // Wildcard route for invalid paths
  {
    path: '**',
    redirectTo: ''
  }
];
