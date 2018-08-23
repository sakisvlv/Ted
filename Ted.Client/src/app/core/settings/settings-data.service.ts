import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment'
import { UserInfo, ChangePassword } from './settings.model';

@Injectable({
  providedIn: 'root'
})
export class SettingsDataService {

  private apiUrl: string = environment.apiUri + "User/";

  constructor(private http: HttpClient) { }


  getProfile() {
    return this.http.get<UserInfo>(this.apiUrl + "UserInfo");
  }

  updateProfile(userInfo: UserInfo) {
    return this.http.put<UserInfo>(this.apiUrl + "UserInfo", userInfo);
  }

  changePassword(userInfo: ChangePassword) {
    return this.http.put<boolean>(this.apiUrl + "ChangePassword", userInfo);
  }


}
