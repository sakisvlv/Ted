import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { SharedModule } from '../../shared/shared.module';

import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth.guard';
import { RoleGuard } from './services/role.guard';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';


@NgModule({
  imports: [
    SharedModule,
    RouterModule,
    HttpClientModule
  ],
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  providers: [
    AuthService,
    AuthGuard,
    RoleGuard

  ],
  exports:
    [
      RegisterComponent
    ]
})
export class AuthModule { }
