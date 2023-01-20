import { EditUserComponent } from './../edit-user/edit-user.component';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { UserModel } from 'src/app/models/user-model';
import { UsersService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit, OnDestroy {

  listData: MatTableDataSource<UserModel>;
  displayedColumns: string[] = ['email', 'actions'];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isMobile: boolean;
  private selectedRowId = '';
  private subscriptions: Subscription = new Subscription();

  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    private usersService: UsersService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog) {}

  ngOnInit(): void {
    this.subscriptions.add(
      this.isHandset.subscribe(
        res => {
          this.isMobile = res;
        }
    ));

    this.usersService.onGet();
    this.subscriptions.add(
      this.usersService.dataSource.subscribe(
        res => {
          this.listData = new MatTableDataSource(res);
          this.listData.sort = this.sort;
          this.listData.paginator = this.paginator;
          if (res.length > 0) {
            if (this.selectedRowId !== '') {
              this.onRowClicked(res.find(e => e.email === this.selectedRowId));
            } else {
              this.onRowClicked(res[0]);
            }
          }
        }
    ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  isSelectedRow(id: string): boolean {
    return this.selectedRowId === id;
  }

  onRowClicked(user: UserModel): void {
    this.selectedRowId = user.email;
    this.usersService.setSelectedRecord(user);
  }

  onEdit(user: UserModel): void {
    this.usersService.populateEditUserForm(user);
    this.dialog.open(EditUserComponent, {
      disableClose: true,
      autoFocus: true,
      width: '100%',
      position: {top: '10px'},
      panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
    });
  }

}
