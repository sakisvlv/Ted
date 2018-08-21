import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';


import { DashboardComponent } from './dashboard/dashboard.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { AdminDataService } from './admin-data.service';

@NgModule({
  imports: [
    SharedModule,
    RouterModule
  ],
  providers: [
    AdminDataService
  ],
  declarations: [
    DashboardComponent,
    UserListComponent,
    UserInfoComponent
  ]
})
export class AdminModule { }
