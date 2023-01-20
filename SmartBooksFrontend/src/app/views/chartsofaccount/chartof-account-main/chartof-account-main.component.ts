import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeFlattener, MatTreeFlatDataSource } from '@angular/material/tree';
import { Subscription, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { TreeLedgerAccountNode, LedgerAccount } from 'src/app/models/ledger-account';
import { ChartofaccountService } from 'src/app/services/chartofaccount.service';
import { ConfirmDialogService } from 'src/app/shared/services/confirm-dialog.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ChartofAccountComponent } from '../chartof-account/chartof-account.component';

@Component({
  selector: 'app-chartof-account-main',
  templateUrl: './chartof-account-main.component.html',
  styleUrls: ['./chartof-account-main.component.css']
})
export class ChartofAccountMainComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription = new Subscription();

  isMobile: boolean;
  private isHandset: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

    treeControl = new FlatTreeControl<TreeLedgerAccountNode>(
      node => node.level, node => node.expandable);

    private transformer = (node: LedgerAccount, level: number) => {
      return {
        expandable: !!node.childAccounts && node.childAccounts.length > 0,
        id: node.id,
        description: node.description,
        accountType: node.accountType,
        accountNumber: node.accountNumber,
        accountName: node.accountName,
        currencyId: node.currencyId,
        parentAccountId: node.parentAccountId,
        taxId: node.taxId,
        cashBookAccountType: node.cashBookAccountType,
        bankId: node.bankId,
        bankBranchId: node.bankBranchId,
        bankAccountNo: node.bankAccountNo,
        hasOverDraft: node.hasOverDraft,
        overDraftLimit: node.overDraftLimit,
        balance: node.balance,
        currencyBalance: node.currencyBalance,
        height: node.height,
        totalBalance: node.totalBalance,
        totalCurrencyBalance: node.totalCurrencyBalance,
        level
      };
    }

    // tslint:disable-next-line: member-ordering
    treeFlattener = new MatTreeFlattener(
      this.transformer, node => node.level, node => node.expandable, node => node.childAccounts);

    // tslint:disable-next-line: member-ordering
    dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    constructor(
      public service: ChartofaccountService,
      private breakpointObserver: BreakpointObserver,
      private dialog: MatDialog,
      private confrimDialog: ConfirmDialogService,
      private notificationService: NotificationService
    ) { }

    ngOnInit(): void {
      this.subscriptions.add(
        this.isHandset.subscribe(
          res => {
            this.isMobile = res;
          }
      ));
      this.service.onGet();
      this.subscriptions.add(
        this.service.dataSource.subscribe(
          res => {
            this.dataSource.data = res;
            this.treeControl.expandAll();
          }
        )
      );
    }

    hasChild = (_: number, node: TreeLedgerAccountNode) => node.expandable;

    ngOnDestroy(): void {
      this.subscriptions.unsubscribe();
    }

      onCreate(): void {
      this.service.initializeFormGroup();
      this.dialog.open(ChartofAccountComponent, {
        disableClose: true,
        autoFocus: true,
        width: '100%',
        position: {top: '10px'},
        panelClass: this.isMobile ? 'form-dialog-container-mobile' : 'form-dialog-container'
      });
    }

    onEdit(row: LedgerAccount): void {
      this.service.populateFormGroup(row);
      this.dialog.open(ChartofAccountComponent, {
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

}
