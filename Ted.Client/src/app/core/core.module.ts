import { NgModule, Optional, SkipSelf, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ImageUploadModule } from "angular2-image-upload";

import { AuthModule } from './auth/auth.module';
import { SharedModule } from '../shared/shared.module'

import { LoaderService } from './loader/loader.service';

import { NavbarComponent } from './navbar/navbar.component';
import { LoaderComponent } from './loader/loader.component';
import { SettingsComponent } from './settings/settings.component';
import { BudgiesService } from './navbar/budgies.service';


@NgModule({
  imports: [
    SharedModule,
    AuthModule,
    RouterModule,
    BrowserAnimationsModule, // required animations module
    ImageUploadModule.forRoot(),
    ToastrModule.forRoot({
      closeButton: true,
      positionClass: 'toast-bottom-right',
      newestOnTop: true
    })
  ],
  declarations: [
    NavbarComponent,
    LoaderComponent,
    SettingsComponent
  ],
  exports: [
    NavbarComponent,
    LoaderComponent,
    AuthModule
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parrentModule: CoreModule) {
    if (parrentModule) {
      throw new Error('CoreModule is already loaded. Import it in the AppModule only');
    }
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        LoaderService,
        BudgiesService
      ]
    }
  }
}
