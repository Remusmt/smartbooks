import { Address } from './../../../models/address';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Customer } from 'src/app/models/customer';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { CustomersService } from 'src/app/services/customers.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CustomerAddressComponent } from '../customer-address/customer-address.component';

@Component({
  selector: 'app-customer-addresses-list',
  templateUrl: './customer-addresses-list.component.html',
  styleUrls: ['./customer-addresses-list.component.css']
})
export class CustomerAddressesListComponent implements OnInit, OnDestroy {

  selectedCustomer: Customer;
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

  listData: MatTableDataSource<Address>;
  displayedColumns: string[] = ['location', 'poBox', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private dialog: MatDialog,
    public service: CustomersService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService) { }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

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
            this.listData = new MatTableDataSource(res.organisationAddresses);
            this.listData.sort = this.sort;
            this.listData.paginator = this.paginator;
          }
      ));
  }

  onEdit(row: Address): void {
    this.service.populateAddressForm(row);
    this.dialog.open(CustomerAddressComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onDelete(rowId: number): void {
    this.subscriptions.add(
      this.confrimDialog.openConfirmDialog('Are you sure to delete this record?')
      .afterClosed().subscribe(res => {
        if (res){
          this.service.onDeleteAddress(rowId)
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

}
