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

const appRoutes: Route[] = [
    {path: '', component: DashboardComponent, children: [
        {path: 'dashboard', component: UserDetailComponent, data: {title: 'Dashboard'}},
        {path: 'groups', component: GroupsComponent, data: {title: 'Groups'}},
        {path: 'friends', component: FriendsComponent, data: {title: 'Friends'}},
        {path: 'messages', component: MessagesComponent, data: {title: 'Messages'}},
        {path: 'available-matches', component: AvailableMatchesComponent, data: {title: 'Available Matches'}},
        {path: 'match-history', component: MatchHistoryComponent, data: {title: 'Match History'}},

    ]},
    {path: 'register', component: RegisterComponent},
    
];


@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports: [RouterModule]

})
export class AppRoutingModule {

}