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

  @Input() changed;

  imageData: any;
  sanitizedImageData: any = "assets/default-user.png";

  constructor(private http: HttpClient,
    private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.getPhoto();
  }


  ngOnChanges(changes: SimpleChanges) {
    // only run when property "data" changed
    if (
      changes['changed']
    ) {
      this.getPhoto();
    }
  }

  getPhoto() {
    this.http.get(this.apiUrl + "DownloadPhoto").subscribe(
      data => {
        this.imageData = 'data:image/png;base64,' + data;
        this.sanitizedImageData = this.sanitizer.bypassSecurityTrustUrl(this.imageData);
      }
    );
  }

}