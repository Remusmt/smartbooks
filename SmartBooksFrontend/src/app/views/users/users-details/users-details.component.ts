import { CreateUserComponent } from './../create-user/create-user.component';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subscription } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UserModel } from 'src/app/models/user-model';
import { UsersService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users-details',
  templateUrl: './users-details.component.html',
  styleUrls: ['./users-details.component.css']
})
export class UsersDetailsComponent implements OnInit, OnDestroy {

  user: UserModel;
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
    public usersService: UsersService,
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
      this.usersService.selectedRecord
        .subscribe(
          res => {
            this.user = res;
          }
      ));

  }

  onSelectedValueChange(event): void {
    this.usersService.setSelectedRecord(event.source.value);
  }

  onCreate(): void {
    this.dialog.open(CreateUserComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

}
