import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewDataService } from './view-data.service';
import { UserSmall } from '../home/home.models';
import { LoaderService } from '../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Experience, Education, PersonalSkill } from '../profile/profile.models';
import { AuthService } from '../core/auth/services/auth.service';

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.scss']
})
export class ViewComponent implements OnInit {

  userId: string = "";
  user: UserSmall;
  connectionsCount: number;
  isFriend: boolean;

  experiences: Experience[] = [];
  educations: Education[] = [];
  personalSkills: PersonalSkill[] = [];

  constructor(
    private route: ActivatedRoute,
    private viewDataService: ViewDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.loaderService.show();
      this.userId = params['id'];

      this.viewDataService.getUserInfo(this.userId).subscribe(
        result => {
          this.user = result;
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );

      this.viewDataService.getConnectionsCount(this.userId).subscribe(
        result => {
          this.connectionsCount = result;
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );

      this.viewDataService.isFriend(this.userId).subscribe(
        result => {
          this.isFriend = result;
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );

      this.viewDataService.getUserSkills(this.userId).subscribe(
        result => {
          this.educations = result.Educations;
          this.educations.forEach(x => x.StartDate = new Date(x.StartDate));
          this.educations.forEach(x => x.EndDate = new Date(x.EndDate));

          this.experiences = result.Experiences;
          this.experiences.forEach(x => x.StartDate = new Date(x.StartDate));
          this.experiences.forEach(x => x.EndDate = new Date(x.EndDate));

          this.personalSkills = result.PersonalSkills;
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );

    });

  }

  message() {
    this.router.navigate(['/conversation', { id: this.userId }]);
  }

}
