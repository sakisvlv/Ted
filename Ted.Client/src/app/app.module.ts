import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DatePipe } from '@angular/common'

import { CoreModule } from './core/core.module';
import { AdminModule } from './admin/admin.module';
import { SharedModule } from './shared/shared.module';

import { AppRoutingModule } from './app-routing.module';

import { TokenInterceptor } from './core/auth/token.interceptor'

import { ProfileDataService } from './profile/profile-data.service';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    AdminModule,
    SharedModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  },
    DatePipe,
    ProfileDataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
