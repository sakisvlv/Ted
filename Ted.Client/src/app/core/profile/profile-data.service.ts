import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ProfileDataService {

  private apiUrl: string = environment.apiUri + "User/";

  constructor(private http: HttpClient) { }

  sendToServer(uploadedFIle: File){
    console.log(uploadedFIle);
    return this.http.post(this.apiUrl + "UploadPhoto", uploadedFIle);
  }


  // getDevices() {
  //   return this.http.get<Device[]>(this.apiUrl + "Devices");
  // }

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
