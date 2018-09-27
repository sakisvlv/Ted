import { Component, OnInit } from '@angular/core';
import { NotificationsDataService } from './notifications-data.service';
import { LoaderService } from '../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Notification } from './notifications.models';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {

  page = 1;
  notifications: Notification[];

  constructor(
    private notificationsDataService: NotificationsDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.notificationsDataService.getNotifications(this.page).subscribe(
      result => {
        this.notifications = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  acknowledgeNotification() {
    this.notificationsDataService.getNotifications(this.page).subscribe(
      result => {
        this.notifications = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

}
