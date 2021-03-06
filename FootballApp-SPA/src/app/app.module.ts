import { SetActiveDirective } from './content-page/messages/set-active.directive';
import { MessagesDisplayContentComponent } from './content-page/messages/messages-display-content/messages-display-content.component';
import { MessagesListUsersComponent } from './content-page/messages/messages-list-users/messages-list-users.component';
import { CreateGroupComponent } from './content-page/groups/create-group/create-group.component';
import { GroupsListComponent } from './content-page/groups/groups-list/groups-list.component';
import { FriendDetailComponent } from './content-page/friends/friends-list/friend-detail/friend-detail.component';
import { FriendsListComponent } from './content-page/friends/friends-list/friends-list.component';
import { MatchHistoryComponent } from './content-page/match-history/match-history.component';
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
import {
  MatTabsModule,
  MatCardModule,
  MatAutocompleteModule,
  MatFormFieldModule,
  MatTooltipModule,
  MatDialogModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatInputModule,
  MatListModule,
  MatRadioModule,
  MatButtonModule,
} from '@angular/material';
import {NgxMaterialTimepickerModule} from 'ngx-material-timepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { TimeAgoPipe } from 'time-ago-pipe';

import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthService } from './_services/auth.service';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { ContentPageComponent } from './content-page/content-page.component';
import { GroupsComponent } from './content-page/groups/groups.component';
import { UserDetailComponent } from './content-page/user-detail/user-detail.component';
import { ToastrModule } from 'ngx-toastr';
import { TokenInterceptor } from './_interceptors/token.interceptor';
import { ConfirmationDialogComponent } from './content-page/groups/groups-list/confirmation-dialog/confirmation-dialog/confirmation-dialog.component';
import { GroupDetailComponent } from './content-page/group-detail/group-detail.component';
import { GroupMembersComponent } from './content-page/group-detail/group-members/group-members.component';
import { GroupPendingAcceptComponent } from './content-page/group-detail/group-pending-accept/group-pending-accept.component';
import { GroupSettingsComponent } from './content-page/group-detail/group-settings/group-settings.component';
import { GroupMatchesComponent } from './content-page/group-detail/group-matches/group-matches.component';
import { GroupAboutComponent } from './content-page/group-detail/group-about/group-about.component';
import { GroupUserBoxComponent } from './content-page/group-detail/group-user-box/group-user-box.component';
import { PlayedMatchComponent } from './content-page/played-match/played-match.component';
import { CreateMatchComponent } from './content-page/group-detail/create-match/create-match.component';
import { UpcomingMatchComponent } from './content-page/upcoming-matches/upcoming-match/upcoming-match.component';
import { UpcomingMatchesComponent } from './content-page/upcoming-matches/upcoming-matches.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { CreateChatModalComponent } from './modals/create-chat-modal/create-chat-modal.component';
import { OrganizeUsersComponent } from './content-page/upcoming-matches/upcoming-match/organize-users/organize-users.component';

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
    CreateChatModalComponent,
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
    MatchHistoryComponent,
    FriendsListComponent,
    FriendDetailComponent,
    ConfirmationDialogComponent,
    GroupDetailComponent,
    GroupMembersComponent,
    GroupPendingAcceptComponent,
    GroupSettingsComponent,
    GroupMatchesComponent,
    GroupAboutComponent,
    GroupUserBoxComponent,
    CreateMatchComponent,
    UpcomingMatchComponent,
    UpcomingMatchesComponent,
    PlayedMatchComponent,
    OrganizeUsersComponent,
    SetActiveDirective,
    TimeAgoPipe,
    NotFoundComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    MatTabsModule,
    MatCardModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatTooltipModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatListModule,
    MatRadioModule,
    MatButtonModule,
    FormsModule,
    NgxMaterialTimepickerModule,
    NgCircleProgressModule.forRoot({
      // set defaults here
      radius: 100,
      outerStrokeWidth: 16,
      innerStrokeWidth: 8,
      outerStrokeColor: '#78C000',
      innerStrokeColor: '#C7E596',
      animationDuration: 300,
    }),
    ReactiveFormsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-left',
      preventDuplicates: true,
    }),
    AppRoutingModule,
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
  ],
  entryComponents: [ConfirmationDialogComponent, CreateChatModalComponent],
  bootstrap: [AppComponent],
})
export class AppModule {}
