import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { Subscription, Observable, merge, of as observableOf } from 'rxjs';
import { map, shareReplay, startWith, switchMap, catchError } from 'rxjs/operators';
import { Customer } from 'src/app/models/customer';
import { CustomersService } from 'src/app/services/customers.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { CustomerComponent } from '../customer/customer.component';
import { CurrentUser } from 'src/app/shared/models/current-user';
import { CustomerAddressComponent } from '../customer-address/customer-address.component';

@Component({
  selector: 'app-customers-list',
  templateUrl: './customers-list.component.html',
  styleUrls: ['./customers-list.component.css']
})
export class CustomersListComponent implements OnDestroy, AfterViewInit {

  displayedColumns: string[] = ['name', 'balance', 'actions'];
  data: Customer[] = [];
  currentUserValue: CurrentUser;

  resultsLength = 0;
  selectedCustomer: Customer;

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
    public service: CustomersService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService)
    {
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
        this.service.onGetItems(this.sort.active, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
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
          if (this.selectedCustomer !== undefined) {
            if (this.selectedCustomer.id > 0) {
              this.onRowClicked(res.find(e => e.id === this.selectedCustomer.id));
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
  }

  onRowClicked(inventoryItem: Customer): void {
    this.service.setSelectedItem(inventoryItem);
  }

  onViewItem(): void {
    this.router.navigate(['/app/customerdetails']);
  }

  isSelectedRow(id: number): boolean {
    return this.selectedCustomer.id === id;
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

  onEdit(row: Customer): void {
    this.service.populateFormGroup(row);
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

}
