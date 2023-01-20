import { BinComponent } from './../bin/bin.component';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Bin, Warehouse } from 'src/app/models/warehouse';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { WarehouseService } from 'src/app/services/warehouse.service';

@Component({
  selector: 'app-bin-list',
  templateUrl: './bin-list.component.html',
  styleUrls: ['./bin-list.component.css']
})
export class BinListComponent implements OnInit, OnDestroy {

  warehouse: Warehouse;
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

  listData: MatTableDataSource<Bin>;
  displayedColumns: string[] = ['code', 'description', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(public warehouseService: WarehouseService,
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
      this.warehouseService.selectedWarehouse
        .subscribe(
          res => {
            this.warehouse = res;
            this.listData = new MatTableDataSource(res.bins);
            this.listData.sort = this.sort;
            this.listData.paginator = this.paginator;
          }
      ));
  }

  onEdit(row: Bin): void {
    this.warehouseService.populateBinFormGroup(row);
    this.dialog.open(BinComponent, {
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
          this.warehouseService.onDeleteBin(rowId)
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
