import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {Location} from '@angular/common';

@Component({
  selector: 'app-customers-main',
  templateUrl: './customers-main.component.html',
  styleUrls: ['./customers-main.component.css']
})
export class CustomersMainComponent {

  isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private breakpointObserver: BreakpointObserver,
    private location: Location
  ) { }

  goBack(): void {
    this.location.back();
  }

}
