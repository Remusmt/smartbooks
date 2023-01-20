import { Supplier } from './../../../models/supplier';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { CompanyService } from 'src/app/services/company.service';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-supplier-address',
  templateUrl: './supplier-address.component.html',
  styleUrls: ['./supplier-address.component.css']
})
export class SupplierAddressComponent implements OnInit, OnDestroy {

  saving = false;
  selectedSupplier: Supplier;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: SuppliersService,
    public companyService: CompanyService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<SupplierAddressComponent>) { }

  ngOnInit(): void {
    this.companyService.onGetCountries();
    this.subscriptions.add(
      this.service.selectedSupplier
        .subscribe(
          res => {
            this.selectedSupplier = res;
          }
      ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.service.addressForm.valid) {
      this.saving = true;
      if (this.service.addressForm.get('id').value !== 0) {
        this.subscriptions.add(
          this.service.onEditAddress(this.service.addressForm.value)
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
          this.service.onCreateAddress(this.service.addressForm.value)
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
    this.service.addressForm.reset();
    this.service.intializeAddressForm(this.selectedSupplier.id, 0);
    this.dialogRef.close();
  }


}
