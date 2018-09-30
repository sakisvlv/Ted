import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { Experience, Post, PostType, Comment, UserSmall, PostMetaData } from './home.models';
import { environment } from '../../environments/environment'
import { Router, Route, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { HomeDataService } from './home-data.service';
import { LoaderService } from '../core/loader/loader.service';
import { AuthService } from '../core/auth/services/auth.service';
import { DomSanitizer } from '@angular/platform-browser';
import { BudgiesService } from '../core/navbar/budgies.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private filesUrl: string = environment.filesUri;

  modal = "closed";

  @ViewChild("article") inputEl: ElementRef;
  PostType = PostType;

  experiance: Experience = new Experience();
  posts: Post[] = [];
  canComment: boolean[] = [];
  isBeingEdited: boolean[] = [];
  connectionsCount = 0;
  postType = PostType.Article;
  title = "";
  description = "";
  comment = new Comment();
  page = 0;
  postId: string;
  showTitle: boolean = false;

  subscribers: UserSmall[] = [];

  selectedFile: File;

  constructor(
    private authService: AuthService,
    private homeDataService: HomeDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService,
    private sanitizer: DomSanitizer,
    private router: Router,
    private route: ActivatedRoute,
    private budgiesService: BudgiesService
  ) { }

  ngOnInit() {
    this.budgiesService.getBudgies();
    this.homeDataService.getLastExperiance().subscribe(
      result => {
        this.experiance = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
      }
    );

    this.homeDataService.getConnectionsCount().subscribe(
      result => {
        this.connectionsCount = result;
      },
      error => {
      }
    );

    this.route.params.subscribe(params => {
      this.loaderService.show();
      this.postId = params['id'];

      if (this.postId != undefined) {
        this.homeDataService.getPost(this.postId).subscribe(
          result => {
            this.posts.push(result);
            this.loaderService.hide();
          },
          error => {
            this.loaderService.hide();
          }
        );
      }
      else {
        this.getPosts();
      }

    });

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
      }
    );
  }

  initPosts() {
    for (let i = 0; i < this.posts.length; i++) {
      this.canComment[i] = false;
      this.isBeingEdited[i] = false;
      if (this.posts[i].Type != this.PostType.Article) {
        this.posts[i].FileUrl = this.filesUrl + this.posts[i].FileName;
      }
    }
  }

  focusInput() {
    this.inputEl.nativeElement.focus();
  }

  post() {

    if (this.postType == PostType.Article) {
      this.homeDataService.postArticle(this.title, this.description).subscribe(
        result => {
          this.posts.unshift(result);
          this.title = "";
        },
        error => {
          this.toastrService.error(error.error, 'Error');
        }
      );
    }
    else {
      this.homeDataService.uploadFile(this.selectedFile, this.postType).subscribe(
        result => {
          let postMetadata = new PostMetaData();
          postMetadata.PostId = result;
          postMetadata.Title = this.title;
          this.homeDataService.sendPostMetadata(postMetadata).subscribe(
            res => {
              if (res.Type != this.PostType.Article) {
                res.FileUrl = this.filesUrl + res.FileName;
              }
              this.posts.unshift(res);
              this.title = "";
              this.description = "";
              this.showTitle = false;
            },
            error => {
              this.toastrService.error(error.error, 'Error');
            }
          );
        },
        error => {
          this.toastrService.error(error.error, 'Error');
        }
      );
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
    if (post.Subscribers != null && post.Subscribers.find(x => x.Id == this.authService.userId) != undefined) {
      return true;
    }
    return false;
  }

  doSubscribeAction(post: Post) {
    if (this.isSubscribed(post)) {
      this.homeDataService.unsubscribeFromPost(post.Id).subscribe(
        result => {
          let subscriber = post.Subscribers.find(x => x.Id == this.authService.userId);
          post.Subscribers.splice(post.Subscribers.indexOf(subscriber), 1);
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
        post.Subscribers.push(result);
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );

  }

  addComment(index: number) {
    for (let i = 0; i < this.posts.length; i++) {
      if (i == index) {
        this.canComment[i] = true;
        continue;
      }
      this.canComment[i] = false;
    }
    this.comment = new Comment()
    this.comment.User = new UserSmall();
    this.comment.User.Id = this.authService.userId;
  }

  postComment(post: Post) {
    this.homeDataService.postComment(this.comment, post.Id).subscribe(
      result => {
        post.Comments.push(result);
        for (let i = 0; i < this.posts.length; i++) {
          this.canComment[i] = false;
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  deleteComment(post: Post, commentId: string) {
    this.homeDataService.deleteComment(commentId).subscribe(
      result => {
        let commnet = post.Comments.find(x => x.Id == commentId);
        post.Comments.splice(post.Comments.indexOf(commnet), 1);
        for (let i = 0; i < this.posts.length; i++) {
          this.canComment[i] = false;
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0]
  }

  navigateToView(id: string) {
    this.router.navigate(['/view', { id: id }]);
  }

  showSubscribers(post: Post) {
    this.subscribers = post.Subscribers;
    this.modal = 'open';
  }

}
