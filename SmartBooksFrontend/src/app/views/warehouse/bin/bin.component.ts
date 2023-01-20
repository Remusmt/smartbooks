import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Warehouse } from 'src/app/models/warehouse';
import { WarehouseService } from 'src/app/services/warehouse.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-bin',
  templateUrl: './bin.component.html',
  styleUrls: ['./bin.component.css']
})
export class BinComponent implements OnInit, OnDestroy {

  saving = false;
  warehouse: Warehouse;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public warehouseService: WarehouseService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<BinComponent>) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.warehouseService.selectedWarehouse
        .subscribe(
          res => {
            this.warehouse = res;
          }
      ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.warehouseService.binForm.valid) {
      this.saving = true;
      if (this.warehouseService.binForm.get('id').value !== 0) {
        this.subscriptions.add(
          this.warehouseService.onEditBin(this.warehouseService.binForm.value)
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
          this.warehouseService.onCreateBin(this.warehouseService.binForm.value)
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
    this.warehouseService.binForm.reset();
    this.warehouseService.initializeBinFormGroup(this.warehouse.id);
    this.dialogRef.close();
  }

}
