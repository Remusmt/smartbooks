import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Tax, TaxRate } from '../models/tax';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class TaxesService {

  controller = 'Taxes';

  tax = {
    id: 0,
    code: '',
    description: '',
    taxRegistrationNumber: '',
    taxAgencyId: null,
    reportingMethod: null,
    purchasesAccountId: 0,
    salesAccountId: 0,
    taxRates: []
  };

  private dataSourceSubject = new BehaviorSubject<Tax[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  private selectedTaxSubject = new BehaviorSubject<Tax>(this.tax);
  public selectedTax = this.selectedTaxSubject.asObservable();

  private taxCountSubject = new BehaviorSubject<number>(0);
  public taxCount = this.taxCountSubject.asObservable();

  public wasFetched: boolean;

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    code:  new FormControl('', Validators.required),
    description:  new FormControl('', Validators.required),
    taxAgencyId: new FormControl('', Validators.required),
    taxRegistrationNumber: new FormControl(''),
    reportingMethod: new FormControl('', Validators.required),
    purchasesAccountId: new FormControl(''),
    salesAccountId: new FormControl('')
  });

  taxRateForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    description:  new FormControl('', Validators.required),
    taxId: new FormControl(''),
    salesRate:  new FormControl(''),
    purchaseRate:  new FormControl(''),
    purchaseTaxIsReclaimable:  new FormControl('')
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) { }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        code:  '',
        description:  '',
        taxRegistrationNumber: '',
        taxAgencyId: null,
        reportingMethod: 0,
        purchasesAccountId: null,
        salesAccountId: null
      });
    }

    populateFormGroup(model: Tax): void {
      this.form.setValue({
        id: model.id,
        code: model.code,
        description: model.description,
        taxRegistrationNumber: model.taxRegistrationNumber,
        taxAgencyId: model.taxAgencyId,
        reportingMethod: model.reportingMethod,
        purchasesAccountId: model.purchasesAccountId,
        salesAccountId: model.salesAccountId
      });
    }

    initializeTaxRateFormGroup(taxId: number): void {
      this.taxRateForm.setValue({
        id: 0,
        description:  '',
        taxId,
        salesRate: 0,
        purchaseRate: 0,
        purchaseTaxIsReclaimable: true
      });
    }

    populateTaxRateFormGroup(model: TaxRate): void {
      this.taxRateForm.setValue({
        id: model.id,
        description:  model.description,
        taxId: model.taxId,
        salesRate: model.salesRate,
        purchaseRate: model.purchaseRate,
        purchaseTaxIsReclaimable: model.purchaseTaxIsReclaimable
      });
    }

    public setSelected(tax: Tax): void {
      this.selectedTaxSubject.next(tax);
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<Tax[]>(`${this.constants.baseUrl}${this.controller}`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onCreate(model: Tax): Observable<Tax> {
      return this.http.post<Tax>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onCreateTaxRate(model: TaxRate): Observable<TaxRate> {
      return this.http.post<TaxRate>(`${this.constants.baseUrl}${this.controller}/CreateTaxRate`, model)
        .pipe(
          map(res => {
            this.selectedTaxSubject.value.taxRates.push(res);
            this.selectedTaxSubject.next(this.selectedTaxSubject.value);
            return res;
          })
        );
    }

    onEdit(model: Tax): Observable<Tax> {
      return this.http.put<Tax>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).code =  res.code;
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEditTaxRate(model: TaxRate): Observable<TaxRate> {
      return this.http.put<TaxRate>(`${this.constants.baseUrl}${this.controller}/UpdateTaxRate`, model)
        .pipe(
          map(res => {
            this.selectedTaxSubject.value.taxRates.find(e => e.id === res.id).description = res.description;
            this.selectedTaxSubject.value.taxRates.find(e => e.id === res.id).purchaseRate = res.purchaseRate;
            this.selectedTaxSubject.value.taxRates.find(e => e.id === res.id).purchaseTaxIsReclaimable = res.purchaseTaxIsReclaimable;
            this.selectedTaxSubject.value.taxRates.find(e => e.id === res.id).salesRate = res.salesRate;
            this.selectedTaxSubject.next(this.selectedTaxSubject.value);
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

    onDeleteTaxRate(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/DeleteTaxRate/${id}`)
        .pipe(
          map(res => {
            const index = this.selectedTaxSubject.value.taxRates.findIndex(e => e.id === res);
            if (index > -1) {
              this.selectedTaxSubject.value.taxRates.splice(index, 1);
              this.selectedTaxSubject.next(this.selectedTaxSubject.value);
            }
            return res;
          })
        );
    }
}
