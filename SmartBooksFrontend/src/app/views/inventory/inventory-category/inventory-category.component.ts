import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { InventoryCategoriesService } from 'src/app/services/inventory-categories.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-inventory-category',
  templateUrl: './inventory-category.component.html',
  styleUrls: ['./inventory-category.component.css']
})
export class InventoryCategoryComponent implements OnDestroy {
  saving = false;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: InventoryCategoriesService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<InventoryCategoryComponent>) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.service.form.valid) {
      this.saving = true;
      if (this.service.form.get('id').value !== 0) {
        this.subscriptions.add(
          this.service.onEdit(this.service.form.value)
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
          this.service.onCreate(this.service.form.value)
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
    this.service.form.reset();
    this.dialogRef.close();
  }

}
