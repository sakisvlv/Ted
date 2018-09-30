import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Ad } from './ad.models';

@Injectable({
  providedIn: 'root'
})
export class AdDataService {

  private apiUrl: string = environment.apiUri + "Job/";

  constructor(
    private http: HttpClient
  ) { }

  AddAd(ad: Ad) {
    return this.http.post<Ad>(this.apiUrl + "AddJob", ad);
  }

  GetAds() {
    return this.http.get<Ad[]>(this.apiUrl + "GetJobs");
  }

  GetMyAds() {
    return this.http.get<Ad[]>(this.apiUrl + "GetMyJobs");
  }

  applyToAd(id: string) {
    return this.http.get<boolean>(this.apiUrl + "applyToAd/" + id);
  }

  deleteAd(id: string) {
    return this.http.get<boolean>(this.apiUrl + "deleteJob/" + id);
  }

}
