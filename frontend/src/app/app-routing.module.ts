import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {ReservationCalendarComponent} from "./reservation-calendar/reservation-calendar.component";
import {isAuthorizedGuard} from "./authentication/is-authorized.guard";
import {AcceptInvitationComponent} from "./accept-invitation/accept-invitation.component";

const routes: Routes = [
  {path: '', redirectTo: 'calendar', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'calendar', component: ReservationCalendarComponent, canActivate: [isAuthorizedGuard]},
  {path: 'accept-invitation', component: AcceptInvitationComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
