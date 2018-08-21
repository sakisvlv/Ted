import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgDatepickerModule } from 'ng2-datepicker';
import { AmazingTimePickerModule } from 'amazing-time-picker';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faSignInAlt,
  faHome,
  faUser,
  faSignOutAlt,
  faCog,
  faDigitalTachograph,
  faHistory,
  faTabletAlt,
  faCalendarAlt,
  faClock,
  faCloudDownloadAlt,
  faUserCircle,
  faTachometerAlt,
  faInfoCircle
} from '@fortawesome/free-solid-svg-icons';

import { PaginationComponent } from './pagination/pagination.component';
import { DateTimePickerComponent } from './date-time-picker/date-time-picker.component';
import { ProfileImageDirective } from './profile-image.directive';

@NgModule({
  imports: [
    CommonModule,
    FontAwesomeModule,
    NgDatepickerModule,
    NgxPaginationModule,
    AmazingTimePickerModule
  ],
  declarations: [
    PaginationComponent,
    DateTimePickerComponent,
    ProfileImageDirective
  ],
  exports: [
    FormsModule,
    CommonModule,
    FontAwesomeModule,
    NgxPaginationModule,
    PaginationComponent,
    DateTimePickerComponent,
    AmazingTimePickerModule,
    ProfileImageDirective
  ],
  providers: []
})
export class SharedModule {

  constructor() {
    this.addFaIcons();
  }

  private addFaIcons() {
    library.add(
      faSignInAlt,
      faHome,
      faUser,
      faSignOutAlt,
      faCog,
      faDigitalTachograph,
      faHistory,
      faTabletAlt,
      faCalendarAlt,
      faClock,
      faCloudDownloadAlt,
      faUserCircle,
      faTachometerAlt,
      faInfoCircle
    );
  }
}
