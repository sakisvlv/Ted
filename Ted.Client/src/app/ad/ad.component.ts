import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../core/loader/loader.service';
import { AdDataService } from './ad-data.service';
import { ToastrService } from 'ngx-toastr';
import { Ad } from './ad.models';

@Component({
  selector: 'app-ad',
  templateUrl: './ad.component.html',
  styleUrls: ['./ad.component.scss']
})
export class AdComponent implements OnInit {
  

  Ads: Ad[];

  constructor(
    private loaderService: LoaderService,
    private adDataService : AdDataService,
    private toastrService : ToastrService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.adDataService.GetAds().subscribe(
      result => {
        this.Ads = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

}
