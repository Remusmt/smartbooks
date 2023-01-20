import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';
import { InventoryCategoriesService } from 'src/app/services/inventory-categories.service';
import { InventoryService } from 'src/app/services/inventory.service';
import { TaxesService } from 'src/app/services/taxes.service';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-inventory-item',
  templateUrl: './inventory-item.component.html',
  styleUrls: ['./inventory-item.component.css']
})
export class InventoryItemComponent implements OnInit, OnDestroy {

  saving = false;
  private subscriptions: Subscription = new Subscription();

  inventoryTypes = [
    {value: 0, description: 'Inventory'},
    {value: 1, description: 'Service'},
    {value: 2, description: 'Non Inventory'},
    {value: 3, description: 'Product'}
  ];

  constructor(
    public service: InventoryService,
    public taxesService: TaxesService,
    private notificationService: NotificationService,
    public unitofMeasureService: UnitofMeasureService,
    public chartofaccountService: ChartofaccountService,
    private dialogRef: MatDialogRef<InventoryItemComponent>,
    public inventoryCategoriesService: InventoryCategoriesService) { }

  ngOnInit(): void {
    this.taxesService.onGet();
    this.unitofMeasureService.onGet();
    this.inventoryCategoriesService.onGet();
    this.chartofaccountService.onGetPostingAccounts();
  }

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
