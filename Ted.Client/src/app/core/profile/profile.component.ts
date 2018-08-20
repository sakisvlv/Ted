import { Component, OnInit } from '@angular/core';

import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  private apiUrl: string = environment.apiUri + "User/";


  constructor() { }

  ngOnInit() {
    
  }


//   var reader = new FileReader();
//  reader.readAsDataURL(blob); 
//  reader.onloadend = function() {
//      base64data = reader.result;                
//      console.log(base64data);
//  }

}
