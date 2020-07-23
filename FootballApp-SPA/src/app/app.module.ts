import { SetActiveDirective } from './content-page/messages/set-active.directive';
import { MessagesDisplayContentComponent } from './content-page/messages/messages-display-content/messages-display-content.component';
import { MessagesListUsersComponent } from './content-page/messages/messages-list-users/messages-list-users.component';
import { CreateGroupComponent } from './content-page/groups/create-group/create-group.component';
import { GroupsListComponent } from './content-page/groups/groups-list/groups-list.component';
import { FriendDetailComponent } from './content-page/friends/friends-list/friend-detail/friend-detail.component';
import { FriendsListComponent } from './content-page/friends/friends-list/friends-list.component';
import { MatchHistoryComponent } from './content-page/match-history/match-history.component';
import { AvailableMatchesComponent } from './content-page/available-matches/available-matches.component';
import { MessagesComponent } from './content-page/messages/messages.component';
import { FriendsComponent } from './content-page/friends/friends.component';
import { UserPhotosComponent } from './content-page/user-detail/user-photos/user-photos.component';
import { UserSettingsComponent } from './content-page/user-detail/user-settings/user-settings.component';
import { UserCommentsComponent } from './content-page/user-detail/user-comments/user-comments.component';
import { UserStatisticsComponent } from './content-page/user-detail/user-statistics/user-statistics.component';
import { UserLatestMatchesComponent } from './content-page/user-detail/user-latest-matches/user-latest-matches.component';
import { UserProfileComponent } from './content-page/user-detail/user-profile/user-profile.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {MatTabsModule, MatCardModule} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgCircleProgressModule } from 'ng-circle-progress';

import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthService } from './_services/auth.service';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { ContentPageComponent } from './content-page/content-page.component';
import { GroupsComponent } from './content-page/groups/groups.component';
import { UserDetailComponent } from './content-page/user-detail/user-detail.component';


@NgModule({
   declarations: [
      AppComponent,
      RegisterComponent,
      DashboardComponent,
      NavbarComponent,
      SidebarComponent,
      ContentPageComponent,
      GroupsComponent,
      GroupsListComponent,
      CreateGroupComponent,
      UserDetailComponent,
      UserProfileComponent,
      UserLatestMatchesComponent,
      UserStatisticsComponent,
      UserCommentsComponent,
      UserSettingsComponent,
      UserPhotosComponent,
      FriendsComponent,
      MessagesComponent,
      MessagesListUsersComponent,
      MessagesDisplayContentComponent,
      AvailableMatchesComponent,
      MatchHistoryComponent,
      FriendsListComponent,
      FriendDetailComponent,
      SetActiveDirective
   ],
   imports: [
      HttpClientModule,
      BrowserModule,
      BrowserAnimationsModule,
      MatTabsModule,
      MatCardModule,
      FormsModule,
      NgCircleProgressModule.forRoot({
         // set defaults here
         radius: 100,
         outerStrokeWidth: 16,
         innerStrokeWidth: 8,
         outerStrokeColor: "#78C000",
         innerStrokeColor: "#C7E596",
         animationDuration: 300,
       }),
       ReactiveFormsModule,
       AppRoutingModule
   ],
   providers: [
      AuthService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
