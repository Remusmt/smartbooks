import { TaxesService } from 'src/app/services/taxes.service';
import { Tax } from './../../../models/tax';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-tax-rate',
  templateUrl: './tax-rate.component.html',
  styleUrls: ['./tax-rate.component.css']
})
export class TaxRateComponent implements OnInit, OnDestroy {

  saving = false;
  isReclaimable = true;
  tax: Tax;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: TaxesService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<TaxRateComponent>) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.service.selectedTax
        .subscribe(
          res => {
            this.tax = res;
          }
      ));
    this.isReclaimable = this.service.taxRateForm.get('purchaseTaxIsReclaimable').value;
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.service.taxRateForm.valid) {
      this.saving = true;
      if (this.service.taxRateForm.get('id').value !== 0) {
        this.subscriptions.add(
          this.service.onEditTaxRate(this.service.taxRateForm.value)
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
          this.service.onCreateTaxRate(this.service.taxRateForm.value)
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
    this.service.taxRateForm.reset();
    this.service.initializeTaxRateFormGroup(this.tax.id);
    this.dialogRef.close();
  }

  onReclaimableChanged(): void {
    this.isReclaimable = !this.isReclaimable;
    this.service.taxRateForm.controls.purchaseTaxIsReclaimable.setValue(this.isReclaimable);
  }

}
