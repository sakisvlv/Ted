import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DatePipe } from '@angular/common'
import { ImageUploadModule } from "angular2-image-upload";

import { VgCoreModule } from 'videogular2/core';
import { VgControlsModule } from 'videogular2/controls';
import { VgOverlayPlayModule } from 'videogular2/overlay-play';
import { VgBufferingModule } from 'videogular2/buffering';

import { CoreModule } from './core/core.module';
import { AdminModule } from './admin/admin.module';
import { SharedModule } from './shared/shared.module';

import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';

import { TokenInterceptor } from './core/auth/token.interceptor'

import { ProfileDataService } from './profile/profile-data.service';
import { HomeDataService } from './home/home-data.service';
import { AdDataService } from './ad/ad-data.service';
import { ConversationDataService } from './conversation/conversation-data.service';
import { NetworkDataService } from './network/network-data.service';
import { NotificationsDataService } from './notifications/notifications-data.service';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { NetworkComponent } from './network/network.component';
import { AdComponent } from './ad/ad.component';
import { ConversationComponent } from './conversation/conversation.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { ViewComponent } from './view/view.component';
import { ViewDataService } from './view/view-data.service';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    NetworkComponent,
    AdComponent,
    ConversationComponent,
    NotificationsComponent,
    ViewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    AdminModule,
    SharedModule,
    RouterModule,
    ImageUploadModule,
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  },
    DatePipe,
    ProfileDataService,
    HomeDataService,
    AdDataService,
    ConversationDataService,
    NetworkDataService,
    NotificationsDataService,
    ViewDataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
