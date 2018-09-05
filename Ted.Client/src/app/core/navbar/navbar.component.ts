import { Component, OnInit } from '@angular/core';

import { AuthService } from '../auth/services/auth.service';

import { LoginData } from '../auth/auth.model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  burgerOpen = false;
  loginData = new LoginData();
  constructor(public authService: AuthService) { }

  ngOnInit() {
  }

  onSubmit(event) {
    if (event.key == "Enter") {
      this.authService.login(this.loginData);
    }
  }

}
