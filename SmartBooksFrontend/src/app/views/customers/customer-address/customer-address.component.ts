import { CompanyService } from './../../../services/company.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Customer } from 'src/app/models/customer';
import { CustomersService } from 'src/app/services/customers.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-customer-address',
  templateUrl: './customer-address.component.html',
  styleUrls: ['./customer-address.component.css']
})
export class CustomerAddressComponent implements OnInit, OnDestroy {

  saving = false;
  selectedCustomer: Customer;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: CustomersService,
    public companyService: CompanyService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<CustomerAddressComponent>) { }

  ngOnInit(): void {
    this.companyService.onGetCountries();
    this.subscriptions.add(
      this.service.selectedCustomer
        .subscribe(
          res => {
            this.selectedCustomer = res;
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
    this.service.intializeAddressForm(this.selectedCustomer.id, 0);
    this.dialogRef.close();
  }

}
