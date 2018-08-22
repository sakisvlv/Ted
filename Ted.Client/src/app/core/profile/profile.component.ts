import { Component, OnInit } from '@angular/core';

import { environment } from '../../../environments/environment';

import { AuthService } from '../auth/services/auth.service';
import { ProfileDataService } from './profile-data.service';
import { UserInfo } from './profile.model';
import { LoaderService } from '../loader/loader.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  private uploadUrl: string = environment.apiUri + "User/UploadPhoto";
  private downloadUrl: string = environment.apiUri + "User/DownloadPhoto";

  canEdit = false;

  changed = false;
  showUploader = false;

  user: UserInfo = new UserInfo();

  myHeaders: { [name: string]: any } = {
    'Authorization': "Bearer " + this.authService.getToken()
  };

  constructor(
    private authService: AuthService,
    private profileDataService: ProfileDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.profileDataService.getProfile().subscribe(
      result => {
        this.user = result;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, "Error");
      }
    );
  }

  onUploadFinished(event) {
    this.showUploader = !this.showUploader;
    this.changed = !this.changed;
  }

  customStyle = {
    selectButton: {
      "color": "white",
      "background-color": "#2780e3",
      "width": "100%"
    },
    layout: {
      "font-size": "9px",
    }
  };

  save() {
    this.loaderService.show();
    this.profileDataService.updateProfile(this.user).subscribe(
      result => {
        this.user = result;
        this.canEdit = false;
        this.loaderService.hide();
        this.toastrService.success("Changes saved", "Success");
      },
      error => {
        this.canEdit = false;
        this.loaderService.hide();
        this.toastrService.error(error.error, "Error");
      }
    );
  }

}
