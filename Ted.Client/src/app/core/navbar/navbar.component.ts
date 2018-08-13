import { Component, OnInit } from '@angular/core';

import { AuthService } from '../auth/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  burgerOpen = false;

  constructor(public authService: AuthService) { }

  ngOnInit() {
  }

}
