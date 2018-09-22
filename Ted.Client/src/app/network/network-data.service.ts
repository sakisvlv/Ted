import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';
import { UserSmall } from '../home/home.models';

@Injectable({
  providedIn: 'root'
})
export class NetworkDataService {

  private apiUrl: string = environment.apiUri + "Network/";

  constructor(private http: HttpClient) { }

  getFriends() {
    return this.http.get<UserSmall[]>(this.apiUrl + "GetFriends");
  }

  getPendingFriends() {
    return this.http.get<UserSmall[]>(this.apiUrl + "GetPendingFriends");
  }

  searchFriends(query: string) {
    return this.http.post<UserSmall[]>(this.apiUrl + "SearchFriends", [query]);
  }

  isFriend(id: string) {
    return this.http.get<boolean>(this.apiUrl + "IsFriend/" + id);
  }

  AddFriend(id: string) {
    return this.http.get<boolean>(this.apiUrl + "AddFriend/" + id);
  }

  AcceptFriend(id: string) {
    return this.http.get<boolean>(this.apiUrl + "AcceptFriend/" + id);
  }

  RejectFriend(id: string) {
    return this.http.get<boolean>(this.apiUrl + "RejectFriend/" + id);
  }

}
