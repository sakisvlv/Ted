import { Component, OnInit } from '@angular/core';

import { environment } from '../../../environments/environment';

import { AuthService } from '../auth/services/auth.service';
import { ProfileDataService } from './profile-data.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  private uploadUrl: string = environment.apiUri + "User/UploadPhoto";
  private downloadUrl: string = environment.apiUri + "User/DownloadPhoto";

  changed = false;
  showUploader = false;

  myHeaders: { [name: string]: any } = {
    'Authorization': "Bearer " + this.authService.getToken()
  };

  constructor(
    private authService: AuthService,
    private profileDataService: ProfileDataService
  ) { }

  ngOnInit() {

  }

  onUploadFinished(event) {
    this.showUploader = !this.showUploader;
    this.changed = !this.changed;
  }

  customStyle = {
    selectButton: {
      "color": "white",
      "background-color": "#2780e3",
      "width":"100%"
    },
    layout: {
      "font-size": "9px",
    }
  };


}
