import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';
import { Experience, Post } from './home.models';

@Injectable({
  providedIn: 'root'
})
export class HomeDataService {

  private apiUrl: string = environment.apiUri + "Home/";

  constructor(private http: HttpClient) { }

  getLastExperiance() {
    return this.http.get<Experience>(this.apiUrl + "LastExperience");
  }

  getPosts() {
    return this.http.get<Post[]>(this.apiUrl + "Posts");
  }

}
