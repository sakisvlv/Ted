import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../core/loader/loader.service';
import { AdDataService } from './ad-data.service';
import { ToastrService } from 'ngx-toastr';
import { Ad } from './ad.models';
import { AuthService } from '../core/auth/services/auth.service';
import { UserSmall } from '../conversation/conversation.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ad',
  templateUrl: './ad.component.html',
  styleUrls: ['./ad.component.scss']
})
export class AdComponent implements OnInit {

  ads: Ad[][] = [];
  all: number;

  myAds: Ad[][] = [];
  my: number;

  applicants: UserSmall[] = [];

  modal = 'closed';
  applicantsModal = 'closed';
  ad: Ad = new Ad();

  constructor(
    private loaderService: LoaderService,
    private adDataService: AdDataService,
    private toastrService: ToastrService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.getAds();
    this.getMyAds();
  }

  getAds() {
    this.adDataService.GetAds().subscribe(
      result => {
        this.all = result.length;
        let current = 0;
        let fives = 0;
        for (let i = 0; i < result.length; i++) {
          if (current == 0) {
            this.ads[fives] = [];
          }
          this.ads[fives][current] = result[i];
          current++;
          if (current == 5) {
            current = 0;
            fives++;
          }
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  getMyAds() {
    this.adDataService.GetMyAds().subscribe(
      result => {
        this.my = result.length;
        let current = 0;
        let fives = 0;
        for (let i = 0; i < result.length; i++) {
          if (current == 0) {
            this.myAds[fives] = [];
          }
          this.myAds[fives][current] = result[i];
          current++;
          if (current == 5) {
            current = 0;
            fives++;
          }
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  adAdd() {
    this.adDataService.AddAd(this.ad).subscribe(
      result => {
        this.ads[0].push(result)
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  showApplicants(ad: Ad) {
    this.applicants = ad.Applicants;
    this.applicantsModal = 'open';
  }

  navigateToView(id: string) {
    this.router.navigate(['/view', { id: id }]);
  }

  applyToAd(ad: Ad) {
    this.adDataService.applyToAd(ad.Id).subscribe(
      result => {
        this.getAds();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  deleteAd(ad: Ad) {
    this.adDataService.deleteAd(ad.Id).subscribe(
      result => {
        this.getMyAds();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }


}
