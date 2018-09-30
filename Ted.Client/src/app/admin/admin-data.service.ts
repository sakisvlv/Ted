import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { UserListItem, Counts } from './admin.models';

import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class AdminDataService {

  private apiUrl: string = environment.apiUri + "Admin/";

  constructor(private http: HttpClient) { }

  getUser(id: string) {
    return this.http.get<UserListItem>(this.apiUrl + "User/" + id);
  }

  getUsers() {
    return this.http.get<UserListItem[]>(this.apiUrl + "Users");
  }

  getCounts() {
    return this.http.get<Counts>(this.apiUrl + "Counts");
  }

  getXml(ids: string[]) {
    return this.http.post<string>(this.apiUrl + "GetXml", ids);
  }



}
