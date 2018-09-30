import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoaderService } from '../loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Budgies } from '../settings/settings.model';
import { environment } from '../../../environments/environment';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { AuthService } from '../auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class BudgiesService {

  hubConnection: HubConnection;

  private apiUrl: string = environment.apiUri + "User/";
  private signalR: string = environment.signalR + "budgies";


  public friendRequests: number = 0;
  public notifications: number = 0;
  public messages: number = 0;

  constructor(
    public http: HttpClient,
    public loaderService: LoaderService,
    public toastrService: ToastrService,
    public authService: AuthService
  ) {

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.signalR, { accessTokenFactory: () => { return this.authService.getToken() } })
      .build();
    this.hubConnection.start().catch(err => console.error(err.toString()));

    this.hubConnection.on('CheckBudgies', (message: string) => {
      this.getBudgies();
    });

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
      }
    );
  }



}
