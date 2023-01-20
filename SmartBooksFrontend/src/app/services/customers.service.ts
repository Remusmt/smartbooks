import { Address } from './../models/address';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Customer, CustomersList } from '../models/customer';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {

  private controller = 'Customers';
  private customer = {
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
    organisationAddresses: []
  };

  private dataSourceSubject = new BehaviorSubject<CustomersList>({organisations: [], totalCount: 0});
  public dataSource = this.dataSourceSubject.asObservable();

  private selectedCustomerSubject = new BehaviorSubject<Customer>(this.customer);
  public selectedCustomer = this.selectedCustomerSubject.asObservable();

  private customerCountSubject = new BehaviorSubject<number>(0);
  public customerCount = this.customerCountSubject.asObservable();

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
    defaultAddressId: new FormControl(null)
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

    public setSelectedItem(customer: Customer): void {
      this.selectedCustomerSubject.next(customer);
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
        defaultAddressId: null
      });
    }

    populateFormGroup(model: Customer): void {
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
        defaultAddressId: model.defaultAddressId
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

    onGetItems(sort: string, order: string, page: number, pageSize: number): void {
      const requestUrl = `${this.constants.baseUrl}${this.controller}/${sort}/${order}/${page + 1}/${pageSize}`;
      this.http.get<CustomersList>(requestUrl)
      .subscribe(
        res => {
          this.dataSourceSubject.next(res);
        }
      );
    }

    onCreate(model: Customer): Observable<Customer> {
      return this.http.post<Customer>(`${this.constants.baseUrl}${this.controller}`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.organisations.concat([res]);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelectedItem(res);
            return res;
          })
        );
    }

    onCreateAddress(model: Address): Observable<Address> {
      return this.http.post<Address>(`${this.constants.baseUrl}${this.controller}/CreateAddress`, model)
        .pipe(
          map(res => {
            this.selectedCustomerSubject.value.organisationAddresses.push(res);
            this.selectedCustomerSubject.next(this.selectedCustomerSubject.value);
            return res;
          })
        );
    }

    onEdit(model: Customer): Observable<Customer> {
      return this.http.put<Customer>(`${this.constants.baseUrl}${this.controller}`, model)
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
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            this.setSelectedItem(res);
            return res;
          })
        );
    }

    onEditAddress(model: Address): Observable<Address> {
      return this.http.put<Address>(`${this.constants.baseUrl}${this.controller}/UpdateAddress`, model)
        .pipe(
          map(res => {
            this.selectedCustomerSubject.value.organisationAddresses.find(e => e.id === res.id).city = res.city;
            this.selectedCustomerSubject.value.organisationAddresses.find(e => e.id === res.id).countryId = res.countryId;
            this.selectedCustomerSubject.value.organisationAddresses.find(e => e.id === res.id).location = res.location;
            this.selectedCustomerSubject.value.organisationAddresses.find(e => e.id === res.id).poBox = res.poBox;
            this.selectedCustomerSubject.value.organisationAddresses.find(e => e.id === res.id).postalCode = res.postalCode;
            this.selectedCustomerSubject.next(this.selectedCustomerSubject.value);
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
            const index = this.selectedCustomerSubject.value.organisationAddresses.findIndex(e => e.id === res);
            if (index > -1) {
              this.selectedCustomerSubject.value.organisationAddresses.splice(index, 1);
              this.selectedCustomerSubject.next(this.selectedCustomerSubject.value);
            }
            return res;
          })
        );
    }

}
