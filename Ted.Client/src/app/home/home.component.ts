import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/auth/services/auth.service';
import { HomeDataService } from './home-data.service';
import { Experience, Post } from './home.models';
import { LoaderService } from '../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  experiance: Experience = new Experience();
  posts: Post[] = [];

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
    this.homeDataService.getPosts().subscribe(
      result => {
        this.posts = result;
        console.log(this.posts);
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }
}
