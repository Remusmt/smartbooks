import { BinComponent } from './../bin/bin.component';
import { WarehouseComponent } from './../warehouse/warehouse.component';
import { WarehouseService } from './../../../services/warehouse.service';
import { Warehouse } from 'src/app/models/warehouse';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-warehouse-details',
  templateUrl: './warehouse-details.component.html',
  styleUrls: ['./warehouse-details.component.css']
})
export class WarehouseDetailsComponent implements OnInit, OnDestroy {

  warehouse: Warehouse;
  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  isMobile: boolean;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public warehouseService: WarehouseService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog) { }

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
          }
      ));

  }

  onSelectedValueChange(event): void {
    this.warehouseService.setSelectedWarehouse(event.source.value);
  }

  onCreateWarehouse(): void {
    this.warehouseService.initializeWarehouseFormGroup();
    this.dialog.open(WarehouseComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onCreateBin(warehouseId: number): void {
    this.warehouseService.initializeBinFormGroup(warehouseId);
    this.dialog.open(BinComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

}
