import { UsersService } from 'src/app/services/user.service';
import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnDestroy {

  saving = false;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public usersService: UsersService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<EditUserComponent>
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.usersService.editUserForm.valid) {
      this.saving = true;
      this.subscriptions.add(
        this.usersService.onEdit(this.usersService.editUserForm.value)
        .subscribe(
          _ => {
            this.notificationService.success(' Updated successfully');
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
