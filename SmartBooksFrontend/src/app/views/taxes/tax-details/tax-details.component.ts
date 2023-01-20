import { TaxRateComponent } from './../tax-rate/tax-rate.component';
import { TaxComponent } from './../tax/tax.component';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { Tax } from 'src/app/models/tax';
import { TaxesService } from 'src/app/services/taxes.service';

@Component({
  selector: 'app-tax-details',
  templateUrl: './tax-details.component.html',
  styleUrls: ['./tax-details.component.css']
})
export class TaxDetailsComponent implements OnInit, OnDestroy {

  tax: Tax;
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
    public service: TaxesService,
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
      this.service.selectedTax
        .subscribe(
          res => {
            this.tax = res;
          }
      ));

  }

  onSelectedValueChange(event): void {
    this.service.setSelected(event.source.value);
  }

  onCreateTax(): void {
    this.service.initializeFormGroup();
    this.dialog.open(TaxComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

  onCreateTaxRate(): void {
    this.service.initializeTaxRateFormGroup(this.tax.id);
    this.dialog.open(TaxRateComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

}
