import { SupplierAddressComponent } from './../supplier-address/supplier-address.component';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { Supplier } from './../../../models/supplier';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Address } from 'src/app/models/address';

@Component({
  selector: 'app-supplier-addresses-list',
  templateUrl: './supplier-addresses-list.component.html',
  styleUrls: ['./supplier-addresses-list.component.css']
})
export class SupplierAddressesListComponent implements OnInit, OnDestroy {

  selectedSupplier: Supplier;
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
    public service: SuppliersService,
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
      this.service.selectedSupplier
        .subscribe(
          res => {
            this.selectedSupplier = res;
            this.listData = new MatTableDataSource(res.organisationAddresses);
            this.listData.sort = this.sort;
            this.listData.paginator = this.paginator;
          }
      ));
  }

  onEdit(row: Address): void {
    this.service.populateAddressForm(row);
    this.dialog.open(SupplierAddressComponent, {
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
