import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { merge, Subscription, of as observableOf, Observable } from 'rxjs';
import { startWith, switchMap, map, catchError, shareReplay } from 'rxjs/operators';
import { InventoryItem } from 'src/app/models/inventory-item';
import { InventoryService } from 'src/app/services/inventory.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { InventoryItemComponent } from '../inventory-item/inventory-item.component';

@Component({
  selector: 'app-inventory-items-list',
  templateUrl: './inventory-items-list.component.html',
  styleUrls: ['./inventory-items-list.component.css']
})
export class InventoryItemsListComponent implements AfterViewInit, OnDestroy {

  displayedColumns: string[] = ['code', 'description', 'onHand', 'actions'];
  data: InventoryItem[] = [];

  resultsLength = 0;
  selectedItem: InventoryItem;

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
    public service: InventoryService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private confrimDialog: ConfirmDialogService,
    private notificationService: NotificationService,
    private router: Router) {
      this.subscriptions.add(
        this.service.selectedItem
          .subscribe(
            res => {
              this.selectedItem = res;
            }
        ));
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

        return data.inventoryItems;
      }),
      catchError(() => {
        return observableOf([]);
      })
    ).subscribe(res => {
      this.data = res;
      if (res.length > 0) {
        if (this.selectedItem !== undefined) {
          if (this.selectedItem.id > 0) {
            this.onRowClicked(res.find(e => e.id === this.selectedItem.id));
          } else {
            this.onRowClicked(res[0]);
          }
        } else {
          this.onRowClicked(res[0]);
        }
      }
    });
  }

  onRowClicked(inventoryItem: InventoryItem): void {
    this.service.setSelectedItem(inventoryItem);
  }

  onViewItem(): void {
    this.router.navigate(['/app/itemdetails']);
  }

  isSelectedRow(id: number): boolean {
    return this.selectedItem.id === id;
  }

  onCreate(): void {
    this.service.initializeFormGroup();
    this.dialog.open(InventoryItemComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onEdit(row: InventoryItem): void {
    this.service.populateFormGroup(row);
    this.dialog.open(InventoryItemComponent, {
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
