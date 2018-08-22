import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment'
import { UserInfo } from './profile.model';

@Injectable({
  providedIn: 'root'
})
export class ProfileDataService {

  private apiUrl: string = environment.apiUri + "User/";

  constructor(private http: HttpClient) { }


  getProfile() {
    return this.http.get<UserInfo>(this.apiUrl + "UserInfo");
  }

  updateProfile(userInfo: UserInfo) {
    return this.http.put<UserInfo>(this.apiUrl + "UserInfo", userInfo);
  }

  // getGpsesByDateTime(deviceId: string, startDateTime: Date, endDateTime: Date) {
  //   let dateTimeWindow = new DateTimeWindow();
  //   dateTimeWindow.StartDateTime = startDateTime;
  //   dateTimeWindow.EndDateTime = endDateTime;
  //   return this.http.post<Gps[]>(this.apiUrl + `Gpses/${deviceId}`, dateTimeWindow);
  // }

  // getHardwareStatusesByDateTime(deviceId: string, startDateTime: Date, endDateTime: Date) {
  //   return this.http.post<HardwareStatus[]>(this.apiUrl + `HardwareStatuses/${deviceId}`, { startDateTime, endDateTime });
  // }

}
