import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { CostcentersService } from 'src/app/services/costcenters.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-costcenter',
  templateUrl: './costcenter.component.html',
  styleUrls: ['./costcenter.component.css']
})
export class CostcenterComponent implements OnDestroy {

  saving = false;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public costcentersService: CostcentersService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<CostcenterComponent>) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.costcentersService.form.valid) {
      this.saving = true;
      if (this.costcentersService.form.get('id').value !== 0) {
        this.subscriptions.add(
          this.costcentersService.onEdit(this.costcentersService.form.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Updated successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              console.log(err);
              this.notificationService.warn(err);
              this.saving = false;
            }
        ));
      } else {
        this.subscriptions.add(
          this.costcentersService.onCreate(this.costcentersService.form.value)
            .subscribe(
              _ => {
                this.notificationService.success(' Saved successfully');
                this.saving = false;
                this.onClose();
              },
              err => {
                console.log(err);
                this.notificationService.warn(err);
                this.saving = false;
              }
        ));
      }
    }
  }

  onClose(): void {
    this.costcentersService.form.reset();
    this.dialogRef.close();
  }

}
