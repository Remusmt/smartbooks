import { SupplierComponent } from './../supplier/supplier.component';
import { Supplier } from './../../../models/supplier';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SupplierAddressComponent } from '../supplier-address/supplier-address.component';

@Component({
  selector: 'app-supplier-details',
  templateUrl: './supplier-details.component.html',
  styleUrls: ['./supplier-details.component.css']
})
export class SupplierDetailsComponent implements OnInit {

  selectedSupplier: Supplier;
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
    public service: SuppliersService,
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
      this.service.selectedSupplier
        .subscribe(
          res => {
            this.selectedSupplier = res;
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
    this.dialog.open(SupplierComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onCreateAddress(): void {
    this.service.intializeAddressForm(this.selectedSupplier.id, this.currentUserValue.companyDefault.defaultCurrency);
    this.dialog.open(SupplierAddressComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(): void {
    if (this.selectedSupplier === undefined) {
      return;
    }
    this.service.populateFormGroup(this.selectedSupplier);
    this.dialog.open(SupplierComponent, {
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
