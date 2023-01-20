import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import {Location} from '@angular/common';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { Category } from 'src/app/models/category';
import { SupplierTypesService } from 'src/app/services/supplier-types.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SupplierTypeComponent } from '../supplier-type/supplier-type.component';

@Component({
  selector: 'app-supplier-types-main',
  templateUrl: './supplier-types-main.component.html',
  styleUrls: ['./supplier-types-main.component.css']
})
export class SupplierTypesMainComponent implements OnInit, OnDestroy {

  listData: MatTableDataSource<Category>;
  displayedColumns: string[] = ['code', 'description', 'actions'];
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
    private dialog: MatDialog,
    private location: Location,
    public service: SupplierTypesService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
    this.service.onGet();
    this.subscriptions.add(
      this.service.dataSource.subscribe(
        res => {
          this.listData = new MatTableDataSource(res);
          this.listData.sort = this.sort;
          this.listData.paginator = this.paginator;
        }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  goBack(): void {
    this.location.back();
  }

  onCreate(): void {
    this.service.initializeFormGroup();
    this.dialog.open(SupplierTypeComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(row: Category): void {
    this.service.populateFormGroup(row);
    this.dialog.open(SupplierTypeComponent, {
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
