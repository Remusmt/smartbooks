import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Customer } from 'src/app/models/customer';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';
import { CompanyService } from 'src/app/services/company.service';
import { CustomerTypesService } from 'src/app/services/customer-types.service';
import { CustomersService } from 'src/app/services/customers.service';
import { PaymentTermsService } from 'src/app/services/payment-terms.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements  OnDestroy, OnInit {

  saving = false;
  selectedCustomer: Customer;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: CustomersService,
    public companyService: CompanyService,
    public paymentTermsService: PaymentTermsService,
    private notificationService: NotificationService,
    public customerTypesService: CustomerTypesService,
    public chartofaccountService: ChartofaccountService,
    private dialogRef: MatDialogRef<CustomerComponent>) { }

  ngOnInit(): void {
    this.companyService.onGetCurrencies();
    this.paymentTermsService.onGet();
    this.customerTypesService.onGet();
    this.chartofaccountService.onGetPostingAccounts();

    this.subscriptions.add(
      this.service.selectedCustomer.subscribe(
        res => {
          this.selectedCustomer = res;
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
