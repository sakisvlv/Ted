import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { Experience, Post, PostType } from './home.models';

import { ToastrService } from 'ngx-toastr';
import { HomeDataService } from './home-data.service';
import { LoaderService } from '../core/loader/loader.service';
import { AuthService } from '../core/auth/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  @ViewChild("article") inputEl: ElementRef;
  PostType = PostType;

  experiance: Experience = new Experience();
  posts: Post[] = [];
  canComment: boolean[] = [];
  isBeingEdited: boolean[] = [];
  connectionsCount = 0;
  postType = PostType.Article;
  title = "";
  page = 0;



  constructor(
    private authService: AuthService,
    private homeDataService: HomeDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.homeDataService.getLastExperiance().subscribe(
      result => {
        this.experiance = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );

    this.getPosts();

    this.homeDataService.getConnectionsCount().subscribe(
      result => {
        this.connectionsCount = result;
      },
      error => {
        this.toastrService.error(error.error, 'Error');
      }
    );

  }

  getPosts() {
    this.homeDataService.getPosts(this.page).subscribe(
      result => {
        this.posts = this.posts.concat(result);
        console.log(this.posts);
        this.initPosts();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  initPosts() {
    for (let i = 0; i < this.posts.length; i++) {
      this.canComment[i] = false;
    }
    for (let i = 0; i < this.posts.length; i++) {
      this.isBeingEdited[i] = false;
    }
  }

  focusInput() {
    this.inputEl.nativeElement.focus();
  }

  post() {
    switch (this.postType) {

      case PostType.Article:
        this.homeDataService.postArticle(this.title).subscribe(
          result => {
            this.posts.unshift(result);
            this.title = "";
          },
          error => {
            this.toastrService.error(error.error, 'Error');
          }
        );
        break;

      case PostType.Article:

        break;

      case PostType.Article:

        break;

    }
  }

  deletePost(post: Post) {
    this.homeDataService.deletePost(post.Id).subscribe(
      result => {
        this.posts.splice(this.posts.indexOf(post), 1);
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  updatePost(post: Post) {
    this.homeDataService.updatePost(post).subscribe(
      result => {
        this.posts[this.posts.indexOf(post)].Title = result.Title;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  editPostTitle(post: Post) {
    this.homeDataService.deletePost(post.Id).subscribe(
      result => {
        this.posts.splice(this.posts.indexOf(post), 1);
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  isSubscribed(post: Post) {
    if (post.Subscribers.find(x => x.Id == this.authService.userId) != undefined) {
      return true;
    }
    return false;
  }

  doSubscribeAction(post: Post) {
    if (this.isSubscribed(post)) {
      this.homeDataService.unsubscribeFromPost(post.Id).subscribe(
        result => {
          let subscriber = post.Subscribers.find(x => x.Id == this.authService.userId);
          post.Subscribers.splice();
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );
      return;
    }
    this.homeDataService.subscribeToPost(post.Id).subscribe(
      result => {
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );

  }

}
