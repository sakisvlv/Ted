import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoaderService } from '../loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Budgies } from '../settings/settings.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgiesService {

  private apiUrl: string = environment.apiUri + "User/";

  public friendRequests: number = 0;
  public notifications: number = 0;
  public messages: number = 0;

  constructor(
    public http: HttpClient,
    public loaderService: LoaderService,
    public toastrService: ToastrService
  ) {
    this.getBudgies();
  }

  getBudgies() {
    this.http.get<Budgies>(this.apiUrl + "Budgies").subscribe(
      res => {
        this.friendRequests = res.FriendRequests;
        this.notifications = res.Notifications;
        this.messages = res.Messages;

        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }



}
