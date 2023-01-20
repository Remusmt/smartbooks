import { Supplier } from './../../../models/supplier';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { TaxesService } from 'src/app/services/taxes.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';

@Component({
  selector: 'app-tax',
  templateUrl: './tax.component.html',
  styleUrls: ['./tax.component.css']
})
export class TaxComponent implements OnInit, OnDestroy {

  saving = false;
  private suppliers: Supplier[] = [];
  private subscriptions: Subscription = new Subscription();

  reportingMethods = [
    {id: 0, description: 'Accrual'},
    {id: 1, description: 'Cash'}
  ];

  constructor(
    public service: TaxesService,
    public suppliersService: SuppliersService,
    private dialogRef: MatDialogRef<TaxComponent>,
    private notificationService: NotificationService,
    public chartofaccountService: ChartofaccountService) { }

  ngOnInit(): void {
    this.suppliersService.onGetTaxAgencies();
    this.chartofaccountService.onGetPostingAccounts();
    this.subscriptions.add(
       this.suppliersService.dataSource.subscribe(
         res => {
          this.suppliers = res.organisations;
         }
       )
     );
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
          this.service.onCreate(this.service.form.value)
            .subscribe(
              _ => {
                this.notificationService.success(' Saved successfully');
                this.saving = false;
                this.onClose();
              },
              err => {
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

  getTaxAngencies(): Supplier[] {
    return this.suppliers.filter(e => e.isTaxAgency);
  }

}
