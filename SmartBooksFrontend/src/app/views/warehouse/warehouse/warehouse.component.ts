import { WarehouseService } from 'src/app/services/warehouse.service';
import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css']
})
export class WarehouseComponent implements OnDestroy {

  saving = false;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public warehouseService: WarehouseService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<WarehouseComponent>) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.warehouseService.warehouseForm.valid) {
      this.saving = true;
      if (this.warehouseService.warehouseForm.get('id').value !== 0) {
        this.subscriptions.add(
          this.warehouseService.onEdit(this.warehouseService.warehouseForm.value)
          .subscribe(
            res => {
              this.notificationService.success(' Updated successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err.error);
              this.saving = false;
            }
        ));
      } else {
        this.subscriptions.add(
          this.warehouseService.onCreate(this.warehouseService.warehouseForm.value)
            .subscribe(
              res => {
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
    this.warehouseService.warehouseForm.reset();
    this.dialogRef.close();
  }

}
