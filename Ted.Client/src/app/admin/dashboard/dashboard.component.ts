import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Counts } from '../admin.models';
import { AdminDataService } from '../admin-data.service';
import { LoaderService } from '../../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(
    private router: Router,
    private adminDataService: AdminDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  counts: Counts;

  ngOnInit() {
    this.adminDataService.getCounts().subscribe(
      result => {
        this.counts = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, "Error");
      }
    )
  }

  redirect(name: string) {
    switch (name) {
      case "users":
        this.router.navigate(['/user-list']);
        break;
    }

  }

}
