import { Tax } from './../../../models/tax';
import { TaxComponent } from './../tax/tax.component';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { TaxesService } from 'src/app/services/taxes.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-taxes-list',
  templateUrl: './taxes-list.component.html',
  styleUrls: ['./taxes-list.component.css']
})
export class TaxesListComponent implements OnInit, OnDestroy {

  listData: MatTableDataSource<Tax>;
  displayedColumns: string[] = ['code', 'description', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isMobile: boolean;
  private selectedTaxId = 0;
  private subscriptions: Subscription = new Subscription();

  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private service: TaxesService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private confrimDialog: ConfirmDialogService,
    private notificationService: NotificationService) {}

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
          if (res.length > 0) {
            if (this.selectedTaxId > 0) {
              this.onRowClicked(res.find(e => e.id === this.selectedTaxId));
            } else {
              this.onRowClicked(res[0]);
            }
          }
        }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  isSelectedRow(id: number): boolean {
    return this.selectedTaxId === id;
  }

  onRowClicked(tax: Tax): void {
    this.selectedTaxId = tax.id;
    this.service.setSelected(tax);
  }

  onEdit(tax: Tax): void {
    this.service.populateFormGroup(tax);
    this.dialog.open(TaxComponent, {
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
