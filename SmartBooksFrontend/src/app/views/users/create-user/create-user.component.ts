import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { UsersService } from 'src/app/services/user.service';
import { ConfirmedValidator } from 'src/app/shared/helpers/custom-password-validator';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnDestroy {

  saving = false;
  private subscriptions: Subscription = new Subscription();

  createUserForm: FormGroup;

   constructor(
    private fb: FormBuilder,
    public usersService: UsersService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<CreateUserComponent>
  ) {
    this.createUserForm = this.fb.group({
      email: new FormControl('', [
        Validators.required,
        Validators.email
      ]),
      fullName: new FormControl('', [
        Validators.required
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6)
      ]),
      confirmPassword: new FormControl('', [
        Validators.required
      ]),
      phoneNumber: new FormControl('')
    }, {
      validator: ConfirmedValidator('password', 'confirmPassword')
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  getEmailErrors(): string {
    if (this.createUserForm.get('email').hasError('required')) {
      return 'Email is required';
    }

    if (this.createUserForm.get('email').hasError('email')) {
      return 'Invalid email';
    }
  }

  getFullNameErrors(): string {
    return this.createUserForm.get('fullName').hasError('required') ? 'Name is required' : '';
  }

  getPasswordErrors(): string {
    if (this.createUserForm.get('password').hasError('required')) {
      return 'Password is required';
    }
    if (this.createUserForm.get('password').errors.minlength) {
      return 'Password must be at least 6 characters long.';
    }
  }

  getConfirmPasswordErrors(): string {
    if (this.createUserForm.get('confirmPassword').hasError('required')) {
      return 'Confrim password required';
    }
    if (this.createUserForm.controls.confirmPassword.errors.confirmedValidator) {
      return 'Password and Confirm Password must be match';
    }
  }

  onSubmit(): void {
    if (this.createUserForm.valid) {
      this.saving = true;
      this.subscriptions.add(
        this.usersService.onCreate(this.createUserForm.value)
        .subscribe(
          _ => {
            this.notificationService.success(' Saved successfully');
            this.saving = false;
            this.onClose();
          },
          err => {
            this.notificationService.warn(err.error);
            this.saving = false;
          }
      ));
    }
  }

  onClose(): void {
    this.usersService.editUserForm.reset();
    this.dialogRef.close();
  }

}
