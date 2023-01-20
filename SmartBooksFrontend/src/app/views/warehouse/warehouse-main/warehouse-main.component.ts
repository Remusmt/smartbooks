import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {Location} from '@angular/common';

@Component({
  selector: 'app-warehouse-main',
  templateUrl: './warehouse-main.component.html',
  styleUrls: ['./warehouse-main.component.css']
})
export class WarehouseMainComponent {

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
