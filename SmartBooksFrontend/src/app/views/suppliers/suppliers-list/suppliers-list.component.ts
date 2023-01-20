import { Supplier } from './../../../models/supplier';
import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { SupplierComponent } from '../supplier/supplier.component';
import { SupplierAddressComponent } from '../supplier-address/supplier-address.component';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { Subscription, Observable, merge, of as observableOf  } from 'rxjs';
import { map, shareReplay, startWith, switchMap, catchError } from 'rxjs/operators';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-suppliers-list',
  templateUrl: './suppliers-list.component.html',
  styleUrls: ['./suppliers-list.component.css']
})
export class SuppliersListComponent implements AfterViewInit, OnDestroy {

  displayedColumns: string[] = ['name', 'balance', 'actions'];
  data: Supplier[] = [];
  currentUserValue: CurrentUser;

  resultsLength = 0;
  selectedSupplier: Supplier;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  private subscriptions: Subscription = new Subscription();

  isMobile: boolean;
  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public service: SuppliersService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService)
    {
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

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
    this.subscriptions.add(
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0)
    );

    merge(this.sort.sortChange, this.paginator.page)
    .pipe(
      startWith({}),
      switchMap(() => {
        this.service.onGetSuppliers(this.sort.active, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
        return this.service.dataSource;
      }),
      map(data => {
        this.resultsLength = data.totalCount;

        return data.organisations;
      }),
      catchError(() => {
        return observableOf([]);
      })
    ).subscribe(res => {
      if (res !== undefined) {
        this.data = res;
        if (res.length > 0) {
          if (this.selectedSupplier !== undefined) {
            if (this.selectedSupplier.id > 0) {
              this.onRowClicked(res.find(e => e.id === this.selectedSupplier.id));
            } else {
              this.onRowClicked(res[0]);
            }
          } else {
            this.onRowClicked(res[0]);
          }
        }
      } else {
        this.data = [];
      }
    });

    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.data = res.organisations;
          this.resultsLength = res.totalCount;
        }
      )
    );
  }

  onRowClicked(supplier: Supplier): void {
    this.service.setSelected(supplier);
  }

  onViewItem(): void {
    this.router.navigate(['/app/supplierdetails']);
  }

  isSelectedRow(id: number): boolean {
    return this.selectedSupplier.id === id;
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

  onEdit(row: Supplier): void {
    this.service.populateFormGroup(row);
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

}
