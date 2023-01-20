import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Address } from '../models/address';
import { SuppliersList, Supplier } from '../models/supplier';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class SuppliersService {

  private controller = 'Suppliers';
  private taxAgentsFetched = false;
  private supplier = {
    id: 0,
    name: '',
    currencyId: null,
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email: '',
    idNumber: '',
    defaultAddressId: null,
    categoryId: null,
    paymentTermId: null,
    creditLimit: 0,
    creditLimitPeriod: 0,
    ledgerAccountId: null,
    balance: 0,
    currencyBalance: 0,
    accountNumber: '',
    vatNumber: '',
    pin: '',
    bankId: null,
    bankBranchId: null,
    bankAccountNumber: '',
    isTaxAgency: false,
    organisationAddresses: []
  };

  private address = {
    location: '',
    poBox: '',
    postalCode: '',
    city: '',
    countryId: null,
    organisationId: 0
  };

  private dataSourceSubject = new BehaviorSubject<SuppliersList>({organisations: [], totalCount: 0});
  public dataSource = this.dataSourceSubject.asObservable();

  private taxAgencyDataSourceSubject = new BehaviorSubject<Supplier[]>([]);
  public taxAgencyDataSource = this.taxAgencyDataSourceSubject.asObservable();

  private selectedSupplierSubject = new BehaviorSubject<Supplier>(this.supplier);
  public selectedSupplier = this.selectedSupplierSubject.asObservable();

  private supplierCountSubject = new BehaviorSubject<number>(0);
  public supplierCount = this.supplierCountSubject.asObservable();

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    name: new FormControl(null, Validators.required),
    currencyId: new FormControl(null, Validators.required),
    accountNumber: new FormControl(null),
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    phoneNumber: new FormControl(null),
    email: new FormControl(null, Validators.email),
    idNumber: new FormControl(null),
    categoryId: new FormControl(null),
    paymentTermId: new FormControl(null),
    creditLimit: new FormControl(null),
    creditLimitPeriod: new FormControl(null),
    ledgerAccountId: new FormControl(null),
    vatNumber: new FormControl(null),
    pin: new FormControl(null),
    defaultAddressId: new FormControl(null),
    bankId: new FormControl(null),
    bankBranchId: new FormControl(null),
    bankAccountNumber: new FormControl(null),
    isTaxAgency: new FormControl(null)
  });

  addressForm: FormGroup = new FormGroup({
    id: new FormControl(null),
    location: new FormControl(null, Validators.required),
    poBox: new FormControl(null),
    postalCode: new FormControl(null),
    city: new FormControl(null),
    countryId: new FormControl(null),
    organisationId: new FormControl(null)
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) {}

    public setSelected(supplier: Supplier): void {
      this.selectedSupplierSubject.next(supplier);
    }

    initializeFormGroup(currency: number): void {
      this.form.setValue({
        id: 0,
        name: '',
        currencyId: currency,
        accountNumber: '',
        firstName: '',
        lastName: '',
        phoneNumber: '',
        email: '',
        idNumber: '',
        categoryId: null,
        paymentTermId: null,
        creditLimit: 0,
        creditLimitPeriod: 0,
        ledgerAccountId: null,
        vatNumber: '',
        pin: '',
        defaultAddressId: null,
        bankId: null,
        bankBranchId: null,
        bankAccountNumber: '',
        isTaxAgency: false
      });
    }

    populateFormGroup(model: Supplier): void {
      this.form.setValue({
        id: model.id,
        name: model.name,
        currencyId: model.currencyId,
        accountNumber: model.accountNumber,
        firstName: model.firstName,
        lastName: model.lastName,
        phoneNumber: model.phoneNumber,
        email: model.email,
        idNumber: model.idNumber,
        categoryId: model.categoryId,
        paymentTermId: model.paymentTermId,
        creditLimit: model.creditLimit,
        creditLimitPeriod: model.creditLimitPeriod,
        ledgerAccountId: model.ledgerAccountId,
        vatNumber: model.vatNumber,
        pin: model.pin,
        defaultAddressId: model.defaultAddressId,
        bankId: model.bankId,
        bankBranchId: model.bankBranchId,
        bankAccountNumber: model.bankAccountNumber,
        isTaxAgency: model.isTaxAgency
      });
    }

    intializeAddressForm(customerId: number, countryId: number): void {
      this.addressForm.setValue({
        id: 0,
        location: '',
        poBox: '',
        postalCode: '',
        city: '',
        organisationId: customerId,
        countryId
      });
    }

    populateAddressForm(model: Address): void {
      this.addressForm.setValue({
        id: model.id,
        location: model.location,
        poBox: model.poBox,
        postalCode: model.postalCode,
        city: model.city,
        countryId: model.countryId,
        organisationId: model.organisationId
      });
    }

    onGetSuppliers(sort: string, order: string, page: number, pageSize: number): void {
      const requestUrl = `${this.constants.baseUrl}${this.controller}/${sort}/${order}/${page + 1}/${pageSize}`;
      this.http.get<SuppliersList>(requestUrl)
      .subscribe(
        res => {
          this.dataSourceSubject.next(res);
        }
      );
    }

    onGetTaxAgencies(): void {
      if (!this.taxAgentsFetched) {
        this.http.get<Supplier[]>(`${this.constants.baseUrl}${this.controller}/GetTaxAngencies`).subscribe(
          res => {
            this.taxAgencyDataSourceSubject.next(res);
            this.taxAgentsFetched = true;
          }
      );
      }
    }

    onCreate(model: Supplier): Observable<Supplier> {
      return this.http.post<Supplier>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.organisations.push(res);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelected(res);
            return res;
          })
        );
    }

    onCreateAddress(model: Address): Observable<Address> {
      return this.http.post<Address>(`${this.constants.baseUrl}${this.controller}/CreateAddress`, model)
        .pipe(
          map(res => {
            this.selectedSupplierSubject.value.organisationAddresses.push(res);
            this.selectedSupplierSubject.next(this.selectedSupplierSubject.value);
            return res;
          })
        );
    }

    onEdit(model: Supplier): Observable<Supplier> {
      return this.http.put<Supplier>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).name =  res.name;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).currencyId =  res.currencyId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).accountNumber =  res.accountNumber;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).firstName =  res.firstName;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).lastName =  res.lastName;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).phoneNumber =  res.phoneNumber;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).email =  res.email;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).idNumber =  res.idNumber;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).categoryId =  res.categoryId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).paymentTermId =  res.paymentTermId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).creditLimit =  res.creditLimit;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).creditLimitPeriod =  res.creditLimitPeriod;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).ledgerAccountId =  res.ledgerAccountId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).vatNumber =  res.vatNumber;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).pin =  res.pin;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).bankId =  res.bankId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).bankBranchId =  res.bankBranchId;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).bankAccountNumber =  res.bankAccountNumber;
            this.dataSourceSubject.value.organisations.find(e => e.id === res.id).isTaxAgency =  res.isTaxAgency;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelected(res);
            return res;
          })
        );
    }

    onEditAddress(model: Address): Observable<Address> {
      return this.http.put<Address>(`${this.constants.baseUrl}${this.controller}/UpdateAddress`, model)
        .pipe(
          map(res => {
            this.selectedSupplierSubject.value.organisationAddresses.find(e => e.id === res.id).city = res.city;
            this.selectedSupplierSubject.value.organisationAddresses.find(e => e.id === res.id).countryId = res.countryId;
            this.selectedSupplierSubject.value.organisationAddresses.find(e => e.id === res.id).location = res.location;
            this.selectedSupplierSubject.value.organisationAddresses.find(e => e.id === res.id).poBox = res.poBox;
            this.selectedSupplierSubject.value.organisationAddresses.find(e => e.id === res.id).postalCode = res.postalCode;
            this.selectedSupplierSubject.next(this.selectedSupplierSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/${id}`)
        .pipe(
          map(res => {
            const index = this.dataSourceSubject.value.organisations.findIndex(e => e.id === res);
            if (index > -1) {
              this.dataSourceSubject.value.organisations.splice(index, 1);
              this.dataSourceSubject.next(this.dataSourceSubject.value);
            }
            return res;
          })
        );
    }

    onDeleteAddress(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}${this.controller}/DeleteAddress/${id}`)
        .pipe(
          map(res => {
            const index = this.selectedSupplierSubject.value.organisationAddresses.findIndex(e => e.id === res);
            if (index > -1) {
              this.selectedSupplierSubject.value.organisationAddresses.splice(index, 1);
              this.selectedSupplierSubject.next(this.selectedSupplierSubject.value);
            }
            return res;
          })
        );
    }
}
