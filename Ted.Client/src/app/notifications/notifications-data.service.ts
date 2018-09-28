import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Notification } from './notifications.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationsDataService {

  private apiUrl: string = environment.apiUri + "Notification/";

  constructor(
    private http: HttpClient
  ) { }

  getNotifications(page: number) {
    return this.http.get<Notification[]>(this.apiUrl + "Notifications/" + page);
  }

  acknowledgeNotification(id: string) {
    return this.http.get<boolean>(this.apiUrl + "AcknowledgeNotification/" + id);
  }

}
