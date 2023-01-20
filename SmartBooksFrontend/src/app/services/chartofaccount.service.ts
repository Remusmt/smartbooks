import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LedgerAccount } from '../models/ledger-account';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class ChartofaccountService {

  controller = 'ChartsofAccount';
  private dataSourceSubject = new BehaviorSubject<LedgerAccount[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  private postingDataSourceSubject = new BehaviorSubject<LedgerAccount[]>([]);
  postingDataSource = this.postingDataSourceSubject.asObservable();

  private wasFetched = false;
  private postingWasFetched = false;

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    accountType: new FormControl('', Validators.required),
    accountNumber:  new FormControl(''),
    accountName:  new FormControl('', Validators.required),
    description:  new FormControl(''),
    currencyId:  new FormControl(''),
    parentAccountId:  new FormControl(''),
    taxId:  new FormControl(''),
    cashBookAccountType:  new FormControl(''),
    bankId:  new FormControl(''),
    bankBranchId:  new FormControl(''),
    bankAccountNo:  new FormControl(''),
    hasOverDraft:  new FormControl(''),
    overDraftLimit:  new FormControl(''),
  });


  constructor(
    private http: HttpClient,
    private constants: ConstantsService) { }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        accountType: null,
        accountNumber: '',
        accountName: '',
        description: '',
        currencyId: 0,
        parentAccountId: null,
        taxId: null,
        cashBookAccountType: 0,
        bankId: null,
        bankBranchId: null,
        bankAccountNo: '',
        hasOverDraft: false,
        overDraftLimit: 0
      });
    }


    populateFormGroup(model: LedgerAccount): void {
      this.form.setValue({
        id: model.id,
        accountType: model.accountType,
        accountNumber: model.accountNumber,
        accountName: model.accountName,
        description: model.description,
        currencyId: model.currencyId,
        parentAccountId: model.parentAccountId,
        taxId: model.taxId,
        cashBookAccountType: model.cashBookAccountType,
        bankId: model.bankId,
        bankBranchId: model.bankBranchId,
        bankAccountNo: model.bankAccountNo,
        hasOverDraft: model.hasOverDraft,
        overDraftLimit: model.overDraftLimit
      });
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<LedgerAccount[]>(`${this.constants.baseUrl}${this.controller}/GetChartsofAccount`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onGetPostingAccounts(forceReload = false): void {
      if (!this.postingWasFetched && !forceReload){
        this.http.get<LedgerAccount[]>(`${this.constants.baseUrl}${this.controller}/GetPostingsAccounts`)
        .subscribe(
          res => {
            this.postingDataSourceSubject.next(res);
            this.postingWasFetched = true;
          }
        );
      }
    }

    onCreate(model: LedgerAccount): Observable<LedgerAccount> {
      return this.http.post<LedgerAccount>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEdit(model: LedgerAccount): Observable<LedgerAccount> {
      return this.http.put<LedgerAccount>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).id =  res.id;
            this.dataSourceSubject.value.find(e => e.id === res.id).accountType =  res.accountType;
            this.dataSourceSubject.value.find(e => e.id === res.id).accountNumber =  res.accountNumber;
            this.dataSourceSubject.value.find(e => e.id === res.id).accountName =  res.accountName;
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.value.find(e => e.id === res.id).currencyId =  res.currencyId;
            this.dataSourceSubject.value.find(e => e.id === res.id).parentAccountId =  res.parentAccountId;
            this.dataSourceSubject.value.find(e => e.id === res.id).taxId =  res.taxId;
            this.dataSourceSubject.value.find(e => e.id === res.id).cashBookAccountType =  res.cashBookAccountType;
            this.dataSourceSubject.value.find(e => e.id === res.id).bankId =  res.bankId;
            this.dataSourceSubject.value.find(e => e.id === res.id).bankBranchId =  res.bankBranchId;
            this.dataSourceSubject.value.find(e => e.id === res.id).bankAccountNo =  res.bankAccountNo;
            this.dataSourceSubject.value.find(e => e.id === res.id).hasOverDraft =  res.hasOverDraft;
            this.dataSourceSubject.value.find(e => e.id === res.id).overDraftLimit =  res.overDraftLimit;
            this.dataSourceSubject.value.find(e => e.id === res.id).balance =  res.balance;
            this.dataSourceSubject.value.find(e => e.id === res.id).currencyBalance =  res.currencyBalance;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/${id}`)
      .pipe(
        map(res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res);
          if (index > -1) {
            this.dataSourceSubject.value.splice(index, 1);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          return res;
        })
      );
    }
}
