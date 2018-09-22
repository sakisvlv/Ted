import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserSmall } from '../home/home.models';
import { environment } from '../../environments/environment';
import { Skills } from '../profile/profile.models';

@Injectable({
  providedIn: 'root'
})
export class ViewDataService {

  private apiUrl: string = environment.apiUri + "View/";

  constructor(private http: HttpClient) { }

  getUserInfo(id: string) {
    return this.http.get<UserSmall>(this.apiUrl + "GetUserInfo/" + id);
  }

  getConnectionsCount(id: string) {
    return this.http.get<number>(this.apiUrl + "GetConnectionsCount/" + id);
  }

  getUserSkills(id: string) {
    return this.http.get<Skills>(this.apiUrl + "Skills/" + id);
  }

  isFriend(id: string) {
    return this.http.get<boolean>(this.apiUrl + "IsFriend/" + id);
  }

}
