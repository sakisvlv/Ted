import { Component, OnInit } from '@angular/core';
import { NotificationsDataService } from './notifications-data.service';
import { LoaderService } from '../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Notification, NotificationType } from './notifications.models';
import { Router } from '@angular/router';
import { BudgiesService } from '../core/navbar/budgies.service';

@Component({
    selector: 'app-notifications',
    templateUrl: './notifications.component.html',
    styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {

    page = 0;
    notifications: Notification[];
    notificationType = NotificationType;

    constructor(
        private notificationsDataService: NotificationsDataService,
        private loaderService: LoaderService,
        private toastrService: ToastrService,
        private router: Router,
        private budgiesService: BudgiesService
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
                this.budgiesService.getBudgies();
            },
            error => {
                this.loaderService.hide();
                this.toastrService.error(error.error, 'Error');
            }
        );
    }

    onClick(notification: Notification) {
        this.notificationsDataService.acknowledgeNotification(notification.Id).subscribe(
            result => {
                notification.IsAcknowledged = true;
                switch (notification.Type) {
                    case this.notificationType.Comment || this.notificationType.Subscribe:
                        this.router.navigate(['/home', { id: notification.PostId }]);
                        break;
                    case this.notificationType.FriendRequest:
                        this.router.navigate(['/network']);
                        break;
                }
                this.loaderService.hide();
            },
            error => {
                this.loaderService.hide();
                this.toastrService.error(error.error, 'Error');
            }
        );
    }

    onShowMore() {
        this.page++;
        this.notificationsDataService.getNotifications(this.page).subscribe(
            result => {
                this.notifications = this.notifications.concat(result);
                this.loaderService.hide();
            },
            error => {
                this.loaderService.hide();
                this.toastrService.error(error.error, 'Error');
            }
        );
    }

}
