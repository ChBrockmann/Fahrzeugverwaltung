import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {ReservationCalendarComponent} from "./reservation-calendar/reservation-calendar.component";
import {isAuthorizedGuard, isUnAuthorizedGuard} from "./authentication/is-authorized.guard";
import {AcceptInvitationComponent} from "./accept-invitation/accept-invitation.component";
import {NotFoundComponent} from "./not-found/not-found.component";

const routes: Routes = [
  {path: '', redirectTo: 'calendar', pathMatch: 'full'},
  {path: 'login', component: LoginComponent, canActivate: [isUnAuthorizedGuard]},
  {path: 'calendar', component: ReservationCalendarComponent, canActivate: [isAuthorizedGuard]},
  {path: 'accept-invitation', component: AcceptInvitationComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
