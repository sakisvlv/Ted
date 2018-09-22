import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgDatepickerModule } from 'ng2-datepicker';
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
    faInfoCircle,
    faIdCard,
    faPhone,
    faEnvelope,
    faLock,
    faUsers,
    faTable,
    faComments,
    faBell,
    faEdit,
    faSave,
    faKey,
    faPlus,
    faPencilAlt,
    faTimes,
    faBackspace,
    faUndoAlt,
    faTrashAlt,
    faVideo,
    faImage,
    faThumbsUp,
    faComment,
    faMusic,
    faPaperPlane
} from '@fortawesome/free-solid-svg-icons';

import { PaginationComponent } from './pagination/pagination.component';
import { DatePickerComponent } from './date-time-picker/date-picker.component';
import { ProfileImageDirective } from './profile-image.directive';

@NgModule({
    imports: [
        CommonModule,
        FontAwesomeModule,
        NgDatepickerModule,
        NgxPaginationModule
    ],
    declarations: [
        PaginationComponent,
        DatePickerComponent,
        ProfileImageDirective
    ],
    exports: [
        FormsModule,
        CommonModule,
        FontAwesomeModule,
        NgxPaginationModule,
        PaginationComponent,
        DatePickerComponent,
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
            faInfoCircle,
            faIdCard,
            faPhone,
            faEnvelope,
            faLock,
            faUsers,
            faTable,
            faComments,
            faBell,
            faEdit,
            faSave,
            faKey,
            faPlus,
            faPencilAlt,
            faTimes,
            faBackspace,
            faUndoAlt,
            faTrashAlt,
            faVideo,
            faImage,
            faThumbsUp,
            faComment,
            faMusic,
            faPaperPlane
        );
    }
}
