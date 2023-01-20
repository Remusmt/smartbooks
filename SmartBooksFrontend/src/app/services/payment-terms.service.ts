import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaymentTerms } from '../models/payment-term';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class PaymentTermsService {

  private dataSourceSubject = new BehaviorSubject<PaymentTerms[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  public wasFetched: boolean;

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    description:  new FormControl('', Validators.required),
    dateDriven:  new FormControl(''),
    netDueIn:  new FormControl(''),
    discountPercentage:  new FormControl(''),
    discountIfPaidWithin:  new FormControl(''),
    netDueBefore:  new FormControl(''),
    dueNextMonthIfIssued:  new FormControl(''),
    discountIfPaidBefore:  new FormControl(''),
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) { }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        description:  '',
        dateDriven: false,
        netDueIn: 0,
        discountPercentage: 0,
        discountIfPaidWithin: 0,
        netDueBefore: 0,
        dueNextMonthIfIssued: 0,
        discountIfPaidBefore: 0
      });
    }

    populateFormGroup(model: PaymentTerms): void {
      this.form.setValue({
        id: model.id,
        description: model.description,
        dateDriven: model.dateDriven,
        netDueIn: model.netDueIn,
        discountPercentage: model.discountPercentage,
        discountIfPaidWithin: model.discountIfPaidWithin,
        netDueBefore: model.netDueBefore,
        dueNextMonthIfIssued: model.dueNextMonthIfIssued,
        discountIfPaidBefore: model.discountIfPaidBefore
      });
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<PaymentTerms[]>(`${this.constants.baseUrl}PaymentTerms`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onCreate(model: PaymentTerms): Observable<PaymentTerms> {
      return this.http.post<PaymentTerms>(`${this.constants.baseUrl}PaymentTerms`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEdit(model: PaymentTerms): Observable<PaymentTerms> {
      return this.http.put<PaymentTerms>(`${this.constants.baseUrl}PaymentTerms`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.value.find(e => e.id === res.id).dateDriven =  res.dateDriven;
            this.dataSourceSubject.value.find(e => e.id === res.id).netDueIn =  res.netDueIn;
            this.dataSourceSubject.value.find(e => e.id === res.id).discountPercentage =  res.discountPercentage;
            this.dataSourceSubject.value.find(e => e.id === res.id).discountIfPaidWithin =  res.discountIfPaidWithin;
            this.dataSourceSubject.value.find(e => e.id === res.id).netDueBefore =  res.netDueBefore;
            this.dataSourceSubject.value.find(e => e.id === res.id).dueNextMonthIfIssued =  res.dueNextMonthIfIssued;
            this.dataSourceSubject.value.find(e => e.id === res.id).discountIfPaidBefore =  res.discountIfPaidBefore;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}PaymentTerms/${id}`)
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
