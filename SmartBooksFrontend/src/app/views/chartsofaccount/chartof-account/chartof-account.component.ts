import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { LedgerAccount } from 'src/app/models/ledger-account';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';
import { CompanyService } from 'src/app/services/company.service';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-chartof-account',
  templateUrl: './chartof-account.component.html',
  styleUrls: ['./chartof-account.component.css']
})
export class ChartofAccountComponent implements OnInit, OnDestroy {

  accountTypes = [
    {value: 1, name: 'Cash', description: 'Create one for each cash account, such as petty cash, saving, checking e.t.c'},
    {value: 2, name: 'Current Asset', description: 'Tracks value of things that can be converted to cash or used up within one year'},
    {value: 3, name: 'Other Assets', description: 'Tracks value of things that are neither fixed or current assets,e.g goodwill'},
    {value: 4, name: 'Accounts Receivable', description: 'Tracks money your customers owe you on unpaid invoices'},
    {value: 5, name: 'Inventory', description: 'Tracks value of things that are produced or purchased for sale'},
    {value: 6, name: 'Fixed Assets', description: 'Tracks the value of signficant items that have a useful life of more than one year'},
    // {value: 7, name: 'Accumulated Depreciation', description: 'Tracks'},
    {value: 8, name: 'Current Liability', description: 'Tracks money your business owes and expects to pay within one year'},
    {value: 9, name: 'Accounts Payable', description: 'Tracks money you owe to vendors for purchases made on credit'},
    {value: 10, name: 'Long Term Liability', description: 'Tracks money your business owes and expects to pay back over more than a year'},
    {value: 11, name: 'Equity', description: 'Tracks money invested in, or money taken out of the business by owners or shareholders'},
    {value: 12, name: 'Income', description: 'Categorises money from normal business operations'},
    {value: 13, name: 'Other Income', description: 'Categorises money your business earns that is unrelated to normal business operations'},
    {value: 14, name: 'Cost of Goods Sold', description: 'Tracks the direct costs to produce the items your business sells'},
    {value: 15, name: 'Expenses', description : 'Categorises money spent in the course of normal business operation'},
    {value: 16, name: 'Other Expenses', description: 'Categorises money your business spends that is unrelated to normal business operations'}
  ];

  accounts: LedgerAccount[];
  accType = 0;
  currentUserValue: CurrentUser;
  saving = false;
  private subscriptions: Subscription = new Subscription();
  typeDescription: string;

  constructor(
    public service: ChartofaccountService,
    public companyService: CompanyService,
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService,
    private dialogRef: MatDialogRef<ChartofAccountComponent>) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.authenticationService.currentUser.subscribe(
        (res: CurrentUser) => {
          this.currentUserValue = res;
        }
    ));
    this.service.onGet();
    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.accounts = res;
        }
    ));
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

  onSelectedValueChange(event): void {
    this.accType = event.source.value;
    this.typeDescription = this.accountTypes
        .find(e => e.value === this.accType).description;
    this.service.form.get('parentAccountId').setValue(null);
    this.service.form.get('currencyId').setValue(this.currentUserValue.companyDefault.defaultCurrency);
  }

  getAccounts(): LedgerAccount[] {
    return this.accounts;
      // .filter(e => e.accountType === this.accType && e.height < 5);
  }

}
