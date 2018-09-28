import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';
import { Experience, Post, UserSmall, Comment, PostType, PostMetaData } from './home.models';

@Injectable({
  providedIn: 'root'
})
export class HomeDataService {
  PostType = PostType;
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

  postArticle(title: string, description: string) {
    return this.http.post<Post>(this.apiUrl + "InsertArticle", [title, description]);
  }

  deletePost(id: string) {
    return this.http.delete<boolean>(this.apiUrl + "DeletePost/" + id);
  }

  updatePost(post: Post) {
    return this.http.put<Post>(this.apiUrl + "UpdatePost", post);
  }

  subscribeToPost(id: string) {
    return this.http.get<UserSmall>(this.apiUrl + "SubscribeToPost/" + id);
  }

  unsubscribeFromPost(id: string) {
    return this.http.get<boolean>(this.apiUrl + "UnsubscribeFromPost/" + id);
  }

  postComment(comment: Comment, postId: string) {
    return this.http.post<Comment>(this.apiUrl + "PostComment/" + postId, comment);
  }

  deleteComment(commentId: string) {
    return this.http.delete<boolean>(this.apiUrl + "DeleteComment/" + commentId);
  }

  uploadFile(selectedFile: File, postType: PostType) {
    const uploadData = new FormData();
    uploadData.append('Image', selectedFile, selectedFile.name);
    switch (postType) {
      case PostType.Image:
        return this.http.post<string>(this.apiUrl + "UploadImage", uploadData);
      case PostType.Audio:
        return this.http.post<string>(this.apiUrl + "UploadAudio", uploadData);
      case PostType.Video:
        return this.http.post<string>(this.apiUrl + "UploadVideo", uploadData);
    }
  }

  getPost(id: string) {
    return this.http.get<Post>(this.apiUrl + "Post/" + id);
  }

  sendPostMetadata(postMetaData: PostMetaData) {
    return this.http.post<Post>(this.apiUrl + "PostMetadata", postMetaData);
  }

  getContent(id: string) {
    return this.http.get<string>(this.apiUrl + "GetContent/" + id);
  }


}
