import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {Location} from '@angular/common';
import { InventoryItem } from 'src/app/models/inventory-item';
import { InventoryService } from 'src/app/services/inventory.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { InventoryItemComponent } from '../inventory-item/inventory-item.component';

@Component({
  selector: 'app-inventory-item-details',
  templateUrl: './inventory-item-details.component.html',
  styleUrls: ['./inventory-item-details.component.css']
})
export class InventoryItemDetailsComponent implements OnInit {

  selectedItem: InventoryItem;
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
    private location: Location,
    public service: InventoryService,
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
      this.service.selectedItem
        .subscribe(
          res => {
            this.selectedItem = res;
          }
      ));
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

  onEdit(): void {
    if (this.selectedItem === undefined) {
      return;
    }
    this.service.populateFormGroup(this.selectedItem);
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


  getTypeDescription(type: number): string {
    switch (type) {
      case 0:
        return 'Inventory';
      case 1:
        return 'Service';
      case 2:
        return 'Non Inventory';
      case 3:
        return 'Product';
    }
  }

  goBack(): void {
    this.location.back();
  }

}
