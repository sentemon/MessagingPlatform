import { Routes } from '@angular/router';
import { AccountComponent } from "./account/account.component";
import { SignInComponent } from "./account/signin/signin.component";
import { SignUpComponent } from "./account/signup/signup.component";
import { SignOutComponent } from "./account/signout/signout.component";

export const routes: Routes = [
  { path: '', redirectTo: 'account/signin', pathMatch: 'full' },
  { path: 'account',
    component: AccountComponent,
    children: [
      { path: 'signin', component: SignInComponent },
      { path: 'signup', component: SignUpComponent },
      { path: 'signout', component: SignOutComponent }
    ]},

  // for invalid route
  { path: '**', redirectTo: '' },
];
