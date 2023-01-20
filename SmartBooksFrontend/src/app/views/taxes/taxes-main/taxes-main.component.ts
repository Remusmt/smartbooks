import { Component } from '@angular/core';
import {Location} from '@angular/common';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-taxes-main',
  templateUrl: './taxes-main.component.html',
  styleUrls: ['./taxes-main.component.css']
})
export class TaxesMainComponent {

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
