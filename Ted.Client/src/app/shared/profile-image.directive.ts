import { Directive, OnInit, Input, SimpleChanges } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ɵBROWSER_SANITIZATION_PROVIDERS, DomSanitizer } from '@angular/platform-browser';

import { environment } from '../../environments/environment';

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

  @Input() changed;
  @Input() id: string = "";

  imageData: any;
  sanitizedImageData: any = "assets/default-user.png";

  constructor(private http: HttpClient,
    private sanitizer: DomSanitizer) { }

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
      this.http.get(this.apiUrl + "DownloadPhoto").subscribe(
        data => {
          this.imageData = 'data:image/png;base64,' + data;
          this.sanitizedImageData = this.sanitizer.bypassSecurityTrustUrl(this.imageData);
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