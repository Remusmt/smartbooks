import { UomConversionComponent } from './../uom-conversion/uom-conversion.component';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UomConversion, UnitofMeasure } from 'src/app/models/unitof-measure';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { EditUomConversionComponent } from '../edit-uom-conversion/edit-uom-conversion.component';

@Component({
  selector: 'app-uom-conversions-list',
  templateUrl: './uom-conversions-list.component.html',
  styleUrls: ['./uom-conversions-list.component.css']
})
export class UomConversionsListComponent implements OnInit, OnDestroy {

  listData: MatTableDataSource<UomConversion>;
  displayedColumns: string[] = ['description', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  unitofMeasure: UnitofMeasure;

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
    public service: UnitofMeasureService,
    private confrimDialog: ConfirmDialogService,
    private breakpointObserver: BreakpointObserver,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));
    this.subscriptions.add(
      this.service.selected.subscribe(
        (res: UnitofMeasure) => {
          this.unitofMeasure = res;
          this.listData = new MatTableDataSource(res.uomConversions);
          this.listData.sort = this.sort;
          this.listData.paginator = this.paginator;
          this.listData.data = this.unitofMeasure.uomConversions;
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onCreate(): void {
    this.service.initializeConversionFormGroup(this.unitofMeasure.id);
    this.dialog.open(UomConversionComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(row: UomConversion): void {
    this.service.populateConversionFormGroup(row);
    this.dialog.open(EditUomConversionComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onDelete(row: UomConversion): void {
    this.subscriptions.add(
      this.confrimDialog.openConfirmDialog('Are you sure to delete this record?')
      .afterClosed().subscribe(res => {
        if (res){
          this.service.onDeleteConversion(row)
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
