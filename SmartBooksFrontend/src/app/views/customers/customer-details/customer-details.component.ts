import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common';
import { Customer } from 'src/app/models/customer';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { CustomersService } from 'src/app/services/customers.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CustomerComponent } from '../customer/customer.component';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { CustomerAddressComponent } from '../customer-address/customer-address.component';

@Component({
  selector: 'app-customer-details',
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {

  selectedCustomer: Customer;
  currentUserValue: CurrentUser;
  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  isMobile: boolean;
  private subscriptions: Subscription = new Subscription();

  constructor(
    private dialog: MatDialog,
    private location: Location,
    public service: CustomersService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
    this.subscriptions.add(
      this.service.selectedCustomer
        .subscribe(
          res => {
            this.selectedCustomer = res;
          }
      ));
    this.authenticationService.currentUser.subscribe(
      (res: CurrentUser) => {
        this.currentUserValue = res;
      }
    );
  }

  onCreate(): void {
    this.service.initializeFormGroup(this.currentUserValue.companyDefault.defaultCurrency);
    this.dialog.open(CustomerComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onCreateAddress(): void {
    this.service.intializeAddressForm(this.selectedCustomer.id, this.currentUserValue.companyDefault.defaultCurrency);
    this.dialog.open(CustomerAddressComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(): void {
    if (this.selectedCustomer === undefined) {
      return;
    }
    this.service.populateFormGroup(this.selectedCustomer);
    this.dialog.open(CustomerComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onDelete(id: number): void {
    this.subscriptions.add(
      this.confrimDialog.openConfirmDialog('Are you sure to delete this record?')
      .afterClosed().subscribe(res => {
        if (res){
          this.service.onDelete(id)
          .subscribe(
            _ => {
              this.notificationService.success(' Deleted successfully');
            },
            err => {
              this.notificationService.warn(err.error);
            }
          );
        }
    }));
  }

  goBack(): void {
    this.location.back();
  }

}
