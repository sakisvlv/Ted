import { Injectable } from '@angular/core';

import { Skills } from './profile.models';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfileDataService {

  private apiUrl: string = environment.apiUri + "User/";

  constructor(private http: HttpClient) { }

  getUserSkills(){
    return this.http.get<Skills>(this.apiUrl + "Skills");
  }

}
