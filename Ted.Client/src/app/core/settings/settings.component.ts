import { Component, OnInit } from '@angular/core';

import { environment } from '../../../environments/environment';

import { AuthService } from '../auth/services/auth.service';
import { UserInfo, ChangePassword } from './settings.model';
import { LoaderService } from '../loader/loader.service';
import { ToastrService } from 'ngx-toastr';
import { SettingsDataService } from './settings-data.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  private uploadUrl: string = environment.apiUri + "User/UploadPhoto";
  private downloadUrl: string = environment.apiUri + "User/DownloadPhoto";

  canEdit = false;
  isChangingPassword = false;

  changed = false;
  showUploader = false;

  user: UserInfo = new UserInfo();
  changePassword: ChangePassword = new ChangePassword();
  confirmPassword: string = "";

  myHeaders: { [name: string]: any } = {
    'Authorization': "Bearer " + this.authService.getToken()
  };

  constructor(
    private authService: AuthService,
    private settingsDataService: SettingsDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.loaderService.show();
    this.settingsDataService.getProfile().subscribe(
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
    localStorage.removeItem("profileImage");
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
    this.settingsDataService.updateProfile(this.user).subscribe(
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

  validate() {
    if (this.changePassword.NewPassword != this.confirmPassword) {
      this.toastrService.error("Οι κωδικοί διαφέρουν", "Σφάλμα");
      return;
    }
    this.settingsDataService.changePassword(this.changePassword).subscribe(
      result => {
        this.toastrService.success("Password changed succsessfully", 'Success');
        this.isChangingPassword = false;
        this.loaderService.hide();
      },
      error => {
        this.isChangingPassword = false;
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );;
  }

}
