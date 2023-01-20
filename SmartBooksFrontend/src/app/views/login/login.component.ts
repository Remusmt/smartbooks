import { CurrentUser } from './../../shared/models/current-user';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { LoginModel } from 'src/app/shared/models/login-model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formGroup: FormGroup;
  serverError = '';
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: AuthenticationService
  ) {
      this.formGroup = fb.group({
        email: new FormControl('',
        [
          Validators.required,
          Validators.email
        ]),
        password: new FormControl('',
        [
          Validators.required
        ])
      });
   }

  ngOnInit(): void {
    if (this.userService.currentUserValue) {
      this.router.navigate(['/app']);
    }
  }

  getEmailErrors(): string {
    if (this.formGroup.get('email').hasError('required')) {
      return 'Email is required';
    }

    if (this.formGroup.get('email').hasError('email')) {
      return 'Invalid email';
    }
  }

  getPasswordErrors(): string {
    if (this.formGroup.get('password').hasError('required')) {
      return 'Password is required';
    }
  }

  submitForm(postModel: LoginModel): void {
    if (this.formGroup.valid) {
      this.serverError = '';
      this.userService.login(postModel)
    .subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.router.navigate(['/app']);
        } else {
          this.serverError = res;
        }
      },
      err => {
        this.serverError = 'Invalid username or password.';
      }
    );
    }
  }

}
