import { UserCommentsComponent } from './content-page/user-detail/user-comments/user-comments.component';
import { UserStatisticsComponent } from './content-page/user-detail/user-statistics/user-statistics.component';
import { UserLatestMatchesComponent } from './content-page/user-detail/user-latest-matches/user-latest-matches.component';
import { UserProfileComponent } from './content-page/user-detail/user-profile/user-profile.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatSliderModule } from '@angular/material/slider';
import {MatTabsModule, MatCardModule, MatButtonModule} from '@angular/material';
import { FormsModule } from '@angular/forms';
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
      UserDetailComponent,
      UserProfileComponent,
      UserLatestMatchesComponent,
      UserStatisticsComponent,
      UserCommentsComponent
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
