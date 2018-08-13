import { Component, OnInit } from '@angular/core';

import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../services/auth.service';

import { RegisterData } from "../auth.model"

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerData: RegisterData = new RegisterData();
  repeatPassword: string;

  constructor(
    private authService: AuthService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
  }

  validate() {
    if (!this.registerData.Email ||
      !this.registerData.FirstName ||
      !this.registerData.LastName ||
      !this.registerData.Password) {
      this.toastrService.error("Όλα τα παιδία είναι υποχρεωτικά", "Σφάλμα");
      return;
    }
    if (this.registerData.Password != this.repeatPassword) {
      this.toastrService.error("Οι κωδικοί διαφέρουν", "Σφάλμα");
      return;
    }
    this.authService.register(this.registerData);
  }

}
