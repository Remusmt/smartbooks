import { TaxRateComponent } from './../tax-rate/tax-rate.component';
import { TaxesService } from 'src/app/services/taxes.service';
import { Tax, TaxRate } from './../../../models/tax';
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

@Component({
  selector: 'app-tax-rates-list',
  templateUrl: './tax-rates-list.component.html',
  styleUrls: ['./tax-rates-list.component.css']
})
export class TaxRatesListComponent implements OnInit, OnDestroy {

  tax: Tax;
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

  listData: MatTableDataSource<TaxRate>;
  displayedColumns: string[] = ['description', 'salesRate', 'purchaseRate', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    public service: TaxesService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private confrimDialog: ConfirmDialogService,
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
      this.service.selectedTax
        .subscribe(
          res => {
            this.tax = res;
            this.listData = new MatTableDataSource(res.taxRates);
            this.listData.sort = this.sort;
            this.listData.paginator = this.paginator;
          }
      ));
  }

  onEdit(row: TaxRate): void {
    this.service.populateTaxRateFormGroup(row);
    this.dialog.open(TaxRateComponent, {
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
          this.service.onDeleteTaxRate(rowId)
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
