import { Component, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service';

import { LoginData } from '../auth.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  
  loginData = new LoginData();

  constructor(public authService: AuthService) { }

  ngOnInit() {
  }

}
