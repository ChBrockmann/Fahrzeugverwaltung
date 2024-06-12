import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {ReservationCalendarComponent} from "./reservation-calendar/reservation-calendar.component";
import {hasRoleGuard, isAuthorizedGuard, isUnAuthorizedGuard} from "./authentication/is-authorized.guard";
import {AcceptInvitationComponent} from "./accept-invitation/accept-invitation.component";
import {NotFoundComponent} from "./not-found/not-found.component";
import {ViewInvitationsComponent} from "./view-invitations/view-invitations.component";
import {environment} from "../environments/environment";

const routes: Routes = [
  {path: '', redirectTo: 'calendar', pathMatch: 'full'},
  {path: 'login', component: LoginComponent, canActivate: [isUnAuthorizedGuard]},
  {path: 'calendar', component: ReservationCalendarComponent, canActivate: [isAuthorizedGuard]},
  {path: 'invitations', component: ViewInvitationsComponent, canActivate: [isAuthorizedGuard, hasRoleGuard], data: {roles: [environment.roles.admin]}},
  {path: 'accept-invitation', component: AcceptInvitationComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
