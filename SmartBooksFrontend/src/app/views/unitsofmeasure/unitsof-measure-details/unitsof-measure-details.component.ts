import { UnitofMeasureComponent } from './../unitof-measure/unitof-measure.component';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UnitofMeasure } from 'src/app/models/unitof-measure';
import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';

@Component({
  selector: 'app-unitsof-measure-details',
  templateUrl: './unitsof-measure-details.component.html',
  styleUrls: ['./unitsof-measure-details.component.css']
})
export class UnitsofMeasureDetailsComponent implements OnInit, OnDestroy {

  selected: UnitofMeasure;
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
    public service: UnitofMeasureService,
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
      this.service.selected
        .subscribe(
          res => {
            this.selected = res;
          }
      ));

  }

  onSelectedValueChange(event): void {
    this.service.setSelected(event.source.value);
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

}
