import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserListItem } from '../admin.models';
import { AdminDataService } from '../admin-data.service';
import { LoaderService } from '../../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss']
})
export class UserInfoComponent implements OnInit {

  userId: string;
  user: UserListItem = new UserListItem();
  constructor(
    private route: ActivatedRoute,
    private adminDataService: AdminDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.route.queryParams.subscribe(params => {
      this.userId = params['id'];
      this.adminDataService.getUser(this.userId).subscribe(
        result => {
          this.user = result;
          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, "Error");
        }
      );
    });
  }

}
