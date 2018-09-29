import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Ad } from './ad.models';

@Injectable({
  providedIn: 'root'
})
export class AdDataService {

  private apiUrl: string = environment.apiUri + "Ad/";

  constructor(
    private http: HttpClient
  ) { }

  AddAd(ad: Ad) {
    return this.http.post<Ad>(this.apiUrl + "AddAd", ad);
  }

  GetAds() {
    return this.http.get<Ad[]>(this.apiUrl + "GetAds");
  }

}
