import { Directive, OnInit, Input, SimpleChanges } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ɵBROWSER_SANITIZATION_PROVIDERS, DomSanitizer } from '@angular/platform-browser';

import { environment } from '../../environments/environment';
import { AuthService } from '../core/auth/services/auth.service';

@Directive({
  selector: '[profile-image]',
  providers: [ɵBROWSER_SANITIZATION_PROVIDERS],
  host: {
    '[src]': 'sanitizedImageData'
  }
})
export class ProfileImageDirective implements OnInit {

  private apiUrl: string = environment.apiUri + "User/";
  private adminApiUrl: string = environment.apiUri + "Admin/";
  private homeApiUrl: string = environment.apiUri + "Home/";

  @Input() changed;
  @Input() id: string = "";

  imageData: any;
  sanitizedImageData: any = "assets/default-user.png";

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer,
    private authService: AuthService
  ) {
  }

  ngOnInit() {

  }


  ngOnChanges(changes: SimpleChanges) {
    // only run when property "data" changed
    if (
      changes['changed'] ||
      (changes['id'] && this.id != undefined)
    ) {
      this.getPhoto();
    }
  }

  getPhoto() {
    if (this.id == "") {
      this.imageData = localStorage.getItem('profileImage');
      if (this.imageData) {
        this.sanitizedImageData = this.sanitizer.bypassSecurityTrustResourceUrl(this.imageData);
        return;
      }
      this.http.get(this.apiUrl + "DownloadPhoto").subscribe(
        data => {
          this.imageData = 'data:image/png;base64,' + data;
          this.sanitizedImageData = this.sanitizer.bypassSecurityTrustUrl(this.imageData);
          localStorage.setItem('profileImage', this.imageData);
        },
        error => {
          this.sanitizedImageData = "assets/default-user.png";
        }
      );
    }
    else if (this.id != "" && this.authService.getRole() == "User") {
      this.http.get(this.homeApiUrl + "DownloadPhoto/" + this.id).subscribe(
        data => {
          if (data != null) {
            this.imageData = 'data:image/png;base64,' + data;
            this.sanitizedImageData = this.sanitizer.bypassSecurityTrustUrl(this.imageData);
          }
          else {
            this.sanitizedImageData = "assets/default-user.png";
          }

        },
        error => {
          this.sanitizedImageData = "assets/default-user.png";
        }
      );
    }
    else {
      this.http.get(this.adminApiUrl + "DownloadPhoto/" + this.id).subscribe(
        data => {
          this.imageData = 'data:image/png;base64,' + data;
          this.sanitizedImageData = this.sanitizer.bypassSecurityTrustUrl(this.imageData);
        },
        error => {
          this.sanitizedImageData = "assets/default-user.png";
        }
      );
    }
  }

}