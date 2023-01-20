import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {Location} from '@angular/common';

@Component({
  selector: 'app-unitsof-measure-main',
  templateUrl: './unitsof-measure-main.component.html',
  styleUrls: ['./unitsof-measure-main.component.css']
})
export class UnitsofMeasureMainComponent {

  isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  constructor(
    private breakpointObserver: BreakpointObserver,
    private location: Location) { }

  goBack(): void {
    this.location.back();
  }
}
