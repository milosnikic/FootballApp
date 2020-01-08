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