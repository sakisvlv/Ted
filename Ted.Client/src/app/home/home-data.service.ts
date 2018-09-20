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

  getPosts(page: number) {
    return this.http.get<Post[]>(this.apiUrl + "Posts/" + page);
  }

  getConnectionsCount() {
    return this.http.get<number>(this.apiUrl + "ConnectionsCount");
  }

  postArticle(article: string) {
    return this.http.post<Post>(this.apiUrl + "InsertArticle", [article]);
  }

  deletePost(id: string) {
    return this.http.delete<boolean>(this.apiUrl + "DeletePost/" + id);
  }

  updatePost(post: Post) {
    return this.http.put<Post>(this.apiUrl + "UpdatePost", post);
  }

  subscribeToPost(id: string) {
    return this.http.get<boolean>(this.apiUrl + "SubscribeToPost/" + id);
  }

  unsubscribeFromPost(id: string) {
    return this.http.get<boolean>(this.apiUrl + "UnsubscribeToPost/" + id);
  }


}
