import { FriendsListComponent } from './content-page/friends/friends-list/friends-list.component';
import { MatchHistoryComponent } from './content-page/match-history/match-history.component';
import { AvailableMatchesComponent } from './content-page/available-matches/available-matches.component';
import { MessagesComponent } from './content-page/messages/messages.component';
import { FriendsComponent } from './content-page/friends/friends.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContentPageComponent } from './content-page/content-page.component';
import { GroupsComponent } from './content-page/groups/groups.component';
import { UserDetailComponent } from './content-page/user-detail/user-detail.component';
import { RegisterComponent } from './register/register.component';
import { NgModule } from '@angular/core';
import { RouterModule, Route } from '@angular/router';
import { GroupDetailComponent } from './content-page/group-detail/group-detail.component';
import { UpcomingMatchComponent } from './content-page/upcoming-match/upcoming-match.component';
import { PlayedMatchComponent } from './content-page/played-match/played-match.component';

const appRoutes: Route[] = [
    {path: 'app', component: DashboardComponent, children: [
        {path: 'dashboard', component: UserDetailComponent, data: {title: 'Dashboard', editable: true}},
        {path: 'groups', component: GroupsComponent, data: {title: 'Groups'}},
        {path: 'groups/:groupId', component: GroupDetailComponent, data: {title: 'Desired group'}},
        {path: 'groups/:groupId/upcoming-match/:matchId', component: UpcomingMatchComponent, data: {title: 'Upcoming match'}},
        {path: 'groups/:groupId/played-match/:matchId', component: PlayedMatchComponent, data: {title: 'Played match'}},
        {path: 'users', component: FriendsComponent, data: {title: 'Users'}},
        {path: 'users/:userId', component: UserDetailComponent, data: {title: 'Users profile', editable: false}},
        {path: 'messages', component: MessagesComponent, data: {title: 'Messages'}},
        {path: 'available-matches', component: AvailableMatchesComponent, data: {title: 'Available Matches'}},
        {path: 'match-history', component: MatchHistoryComponent, data: {title: 'Match History'}},
    ]},
    {path: '', component: RegisterComponent},

];


@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports: [RouterModule]

})
export class AppRoutingModule {

}