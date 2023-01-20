import { SupplierTypesService } from './../../../services/supplier-types.service';
import { AuthenticationService } from './../../../shared/services/authentication.service';
import { BankBranch } from './../../../models/bank';
import { Supplier } from './../../../models/supplier';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';
import { CompanyService } from 'src/app/services/company.service';
import { PaymentTermsService } from 'src/app/services/payment-terms.service';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.css']
})
export class SupplierComponent implements OnInit, OnDestroy {

  saving = false;
  bankBranches: BankBranch[] = [];
  selectedSupplier: Supplier;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: SuppliersService,
    public companyService: CompanyService,
    public paymentTermsService: PaymentTermsService,
    private notificationService: NotificationService,
    public supplierTypesService: SupplierTypesService,
    private dialogRef: MatDialogRef<SupplierComponent>,
    public chartofaccountService: ChartofaccountService,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.companyService.onGetBanks(this.authenticationService.currentUserValue.companyDefault.defaultCurrency);
    this.companyService.onGetCurrencies();
    this.paymentTermsService.onGet();
    this.supplierTypesService.onGet();
    this.chartofaccountService.onGetPostingAccounts();

    this.subscriptions.add(
      this.service.selectedSupplier.subscribe(
        res => {
          this.selectedSupplier = res;
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

  onBankSelected(event): void {
    console.log(event);
    this.bankBranches = [];
  }

  onClose(): void {
    this.service.form.reset();
    this.dialogRef.close();
  }

}
