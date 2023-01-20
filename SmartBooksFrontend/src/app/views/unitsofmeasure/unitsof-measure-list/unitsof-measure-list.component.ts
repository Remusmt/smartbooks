import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UnitofMeasure } from 'src/app/models/unitof-measure';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UnitofMeasureComponent } from '../unitof-measure/unitof-measure.component';

@Component({
  selector: 'app-unitsof-measure-list',
  templateUrl: './unitsof-measure-list.component.html',
  styleUrls: ['./unitsof-measure-list.component.css']
})
export class UnitsofMeasureListComponent implements OnInit, OnDestroy {

  listData: MatTableDataSource<UnitofMeasure>;
  displayedColumns: string[] = ['abbreviation', 'description', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isMobile: boolean;
  private selectedId = 0;
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
    private service: UnitofMeasureService,
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
            if (this.selectedId > 0) {
              this.onRowClicked(res.find(e => e.id === this.selectedId));
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
    return this.selectedId === id;
  }

  onRowClicked(uom: UnitofMeasure): void {
    this.selectedId = uom.id;
    this.service.setSelected(uom);
  }


  onCreate(): void {
    this.service.initializeFormGroup();
    this.dialog.open(UnitofMeasureComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(uom: UnitofMeasure): void {
    this.service.populateFormGroup(uom);
    this.dialog.open(UnitofMeasureComponent, {
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
