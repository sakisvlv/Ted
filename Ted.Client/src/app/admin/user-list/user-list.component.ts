import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserListItem } from '../admin.models';

import { AdminDataService } from '../admin-data.service';
import { LoaderService } from '../../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  private filesUrl: string = environment.filesUri;

  constructor(
    private adminDataService: AdminDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService,
    private router: Router
  ) { }
  users: UserListItem[] = [];
  checkedIds: string[] = [];

  ngOnInit() {
    this.loaderService.show();
    this.adminDataService.getUsers().subscribe(
      result => {
        this.users = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, "Error");
      }
    );
  }

  onCheck(id: string) {
    if (this.checkedIds.find(x => x == id) != undefined) {
      this.checkedIds.splice(this.checkedIds.indexOf(this.checkedIds.find(x => x == id)), 1);
      return;
    }
    this.checkedIds.push(id);
  }

  isChecked(id: string) {
    if (this.checkedIds.find(x => x == id) != undefined) {
      return true;
    }
    return false;
  }

  redirect(id: string) {
    this.router.navigate([`/user-info`], { queryParams: { id: id } });
  }

  getXml() {
    this.adminDataService.getXml(this.checkedIds).subscribe(
      result => {
        console.log(this.filesUrl + result + ".xml");
        
        window.open(this.filesUrl  + result + ".xml");
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, "Error");
      }
    );
  }

}